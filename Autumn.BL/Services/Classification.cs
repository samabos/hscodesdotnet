using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autumn.BL.Models.Request.V3;
using Autumn.BL.Models.Response.V3;
using Autumn.Domain.Models;
using Autumn.Service.Interface;
using Autumn_UIML.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Autumn.Service
{
    public class Classification : IClassification
    {
        private readonly IHsCodeService _hsCodeService;
        private readonly IProductService _productService;
        private readonly ISearchLogService _searchlogService;
        private IConfiguration _configuration;

        public Classification(IConfiguration configuration, IHsCodeService hsCodeService
            , IProductService productService
            , ISearchLogService searchlogService)
        {
            _hsCodeService = hsCodeService;
            _productService = productService;
            _configuration = configuration;
            _searchlogService = searchlogService;
        }

        public async Task<BLSearchResponse> SearchAsync(BLSearchRequest request)
        {
            try
            {
                var response = new BLSearchResponse { Success = true };
                var records = new Dictionary<string, List<BLResultModel>>();

                // Navigation mode — handle separately
                if (!string.IsNullOrEmpty(request.settings))
                {
                    var rm = new BLResultModel
                    {
                        Prediction = string.Empty,
                        Code = request.pid,
                        Rating = 0,
                        Tags = new List<string>(),
                        PHSCodes = new List<HSCode>(),
                        HSCodes = new List<HSCode>()
                    };
                    var rms = new List<BLResultModel>();
                    return await Navigation(request, response, rm, rms, records);
                }

                // --- Blended search: run DB stages concurrently ---
                var allResults = new List<BLResultModel>();

                // DB stages in parallel: exact match + Atlas Search + description
                var exactTask = _productService.GetByKeywordAsync(request.keyword);
                var searchTask = _productService.SearchByKeywordAsync(request.keyword);
                var descTask = _hsCodeService.SearchByDescriptionAsync(request.keyword);

                await Task.WhenAll(exactTask, searchTask, descTask);

                var exactProducts = exactTask.Result ?? new List<Product>();
                var searchProducts = searchTask.Result ?? new List<Product>();
                var hsResults = descTask.Result ?? new List<HSCode>();

                // Process exact matches (highest confidence: 0.88–0.97)
                var exactRms = new List<BLResultModel>();
                for (var pi = 0; pi < exactProducts.Count; pi++)
                {
                    var conf = Math.Max(0.88f, 0.97f - pi * 0.03f);
                    await LoadProduct(exactRms, exactProducts[pi], conf);
                }
                allResults.AddRange(exactRms);

                // Process Atlas Search / regex matches (medium confidence: 0.60–0.82)
                var searchRms = new List<BLResultModel>();
                for (var ri = 0; ri < searchProducts.Count; ri++)
                {
                    var conf = Math.Max(0.60f, 0.82f - ri * 0.02f);
                    await LoadProduct(searchRms, searchProducts[ri], conf);
                }
                allResults.AddRange(searchRms);

                // Process description matches (lower confidence: 0.40–0.73)
                for (var hi = 0; hi < hsResults.Count; hi++)
                {
                    var hs = hsResults[hi];
                    var levelBonus = hs.Level == 4 ? 0.05f : 0f;
                    var conf = Math.Max(0.40f, 0.68f + levelBonus - hi * 0.02f);
                    var rm = new BLResultModel
                    {
                        Prediction = hs.Description,
                        Code = hs.Code,
                        Rating = conf,
                        Tags = new List<string>(),
                        HSCodes = new List<HSCode> { hs },
                        PHSCodes = new List<HSCode>()
                    };
                    // Build ancestor chain
                    var ancestors = new List<HSCode>();
                    var cur = hs;
                    while (cur != null && !string.IsNullOrEmpty(cur.ParentId))
                    {
                        var parents = await _hsCodeService.GetWithOptionsAsync(cur.ParentId, null, null);
                        var parent = parents.FirstOrDefault();
                        if (parent != null)
                        {
                            ancestors.Insert(0, parent);
                            cur = parent;
                        }
                        else break;
                    }
                    rm.PHSCodes = ancestors;
                    allResults.Add(rm);
                }

                // Groq fallback: only call LLM if DB stages returned no high-confidence results
                var bestConfidence = allResults.Count > 0 ? allResults.Max(r => r.Rating) : 0f;
                if (bestConfidence < 0.7f)
                {
                    var groqResults = await GroqClassifyAsync(request.keyword);
                    allResults.AddRange(groqResults);
                }

                // Deduplicate by HS code, keeping the highest-confidence entry
                var merged = allResults
                    .GroupBy(r => r.Code ?? r.HSCodes?.FirstOrDefault()?.Code ?? "")
                    .Where(g => !string.IsNullOrEmpty(g.Key))
                    .Select(g => g.OrderByDescending(r => r.Rating).First())
                    .OrderByDescending(r => r.Rating)
                    .Take(20)
                    .ToList();

                if (merged.Count > 0)
                {
                    records["match"] = merged;
                    response.Records = records;
                    return response;
                }

                // Fallback: synonyms (only if no results from primary stages)
                var synonyms = await GetMatchSynonyms(request.keyword);
                if (synonyms.Count > 0)
                {
                    var synRms = new List<BLResultModel>();
                    for (var si = 0; si < synonyms.Count; si++)
                    {
                        var conf = Math.Max(0.35f, 0.58f - si * 0.03f);
                        await LoadProduct(synRms, synonyms[si], conf);
                    }
                    records["synonym"] = synRms;
                    response.Records = records;
                    return response;
                }

                // Last resort: ML model prediction
                try
                {
                    var aiRms = new List<BLResultModel>();
                    await AIMethod(request, response, aiRms);
                    if (aiRms.Count > 0)
                        records["ai"] = aiRms;
                }
                catch
                {
                    // ML model may not be available
                }

                response.Records = records;
                return response;
            }
            catch (Exception ex)
            {
                return new BLSearchResponse { Success = false, Error = new[] { ex.Message } };
            }
        }

        private async Task<BLResultModel> LoadProduct(List<BLResultModel> rms, Product product, float confidence = 0.5f)
        {
            BLResultModel rm = new BLResultModel();
            rm.Rating = confidence;
            rm.Tags = new List<string>();
            rm.PHSCodes = new List<HSCode>();
            rm.HSCodes = await _hsCodeService.GetWithHSCodeOptionsAsync(product.Code, null, null);
            rm.Prediction = product.Keyword;
            rm.Code = product.Code;
            if (product.Tags != null)
                rm.Tags.AddRange(product.Tags);
            // Build full ancestor chain (Section → Chapter → Heading)
            if (rm.HSCodes.Count > 0)
            {
                var ancestors = new List<HSCode>();
                var current = rm.HSCodes.FirstOrDefault();
                while (current != null && !string.IsNullOrEmpty(current.ParentId))
                {
                    var parents = await _hsCodeService.GetWithOptionsAsync(current.ParentId, null, null);
                    var parent = parents.FirstOrDefault();
                    if (parent != null)
                    {
                        ancestors.Insert(0, parent);
                        current = parent;
                    }
                    else break;
                }
                rm.PHSCodes = ancestors;
            }
            rms.Add(rm);
            return rm;
        }

        private async Task<List<Product>> GetMatchSynonyms(string keyword)
        {
            var collector = new List<Product>();
            var synonyms = await GetSynonyms(keyword.ToLower());
            foreach (var synonym in synonyms.ToList())
            {
                List<Product> productExist = await _productService.GetLikeKeywordAsync(synonym) ?? new List<Product>();
                //if (productExist.Count == 0) synonyms.Remove(synonym);
                collector.AddRange(productExist);

            }
            return collector;

        }

        private async Task<List<string>> GetSynonyms(string keyword)
        {
            try
            {
                var client = new RestClient($"https://languagetools.p.rapidapi.com/synonyms/{keyword}");
                var req = new RestRequest { Method = Method.Get };
                req.AddHeader("content-type", "application/json");
                req.AddHeader("x-rapidapi-key", "9ee21c90d1msh439d7b7bfcaac38p1829eejsn47eaeccaea66");
                req.AddHeader("x-rapidapi-host", "languagetools.p.rapidapi.com");
                var resp = await client.ExecuteAsync(req);
                var synonyms = JsonConvert.DeserializeObject<SynonymModel>(resp.Content);
                return synonyms?.Synonyms ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        private async Task<BLResultModel> AIMethod(BLSearchRequest request, BLSearchResponse response, List<BLResultModel> rms)
        {
            BLResultModel rm;
            var ai = GetHSCode(request.keyword, double.Parse(_configuration["SiteSettings:Threshold"]));
            //if (ai.Count > 0) response.ai = true;

            rm = new BLResultModel
            {
                HSCodes = new List<HSCode>()
            };
            foreach (var item in ai)
            {
                var aiarr = item.Key.Split('-');
                rm.HSCodes.AddRange(await _hsCodeService.GetWithHSCodeOptionsAsync(aiarr[1], null, null));
                rm.Rating = item.Value;
            }

            rm.Prediction = request.keyword;
            rm.PHSCodes = new List<HSCode>();
            rm.Tags = new List<string>();
            rms.Add(rm);
            return rm;
        }

        private async Task<BLSearchResponse> Navigation(BLSearchRequest request, BLSearchResponse response, BLResultModel rm, List<BLResultModel> rms, Dictionary<string, List<BLResultModel>> records)
        {
            if (request.settings == "nav")
            {
                rm.HSCodes = await _hsCodeService.GetWithOptionsAsync(request.id, request.pid, request.level);
                if (!string.IsNullOrEmpty(request.pid))
                    rm.PHSCodes = await _hsCodeService.GetWithOptionsAsync(request.pid, null, null);
                    rm.PHSCodes = await _hsCodeService.GetWithOptionsAsync(request.pid, null, null);

            }
            else if (request.settings == "tag")
            {
                rm.HSCodes = await _hsCodeService.GetWithHSCodeOptionsAsync(request.id, request.pid, request.level);
                if (!string.IsNullOrEmpty(request.pid))
                    rm.PHSCodes = await _hsCodeService.GetWithHSCodeOptionsAsync(request.pid, null, null);
            }

            rms.Add(rm);
            records.Add("nav", rms);
            response.Records = records;
            return response;
        }

        private async Task<List<BLResultModel>> GroqClassifyAsync(string keyword)
        {
            var results = new List<BLResultModel>();
            var apiKey = _configuration["SiteSettings:GroqApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                return results;

            try
            {
                var model = _configuration["SiteSettings:GroqModel"] ?? "llama-3.1-8b-instant";
                var client = new RestClient("https://api.groq.com/openai/v1/chat/completions");
                var req = new RestRequest { Method = Method.Post };
                req.AddHeader("Authorization", $"Bearer {apiKey}");
                req.AddHeader("Content-Type", "application/json");

                var systemPrompt = @"You are an HS Code classification expert. Given a product description, return the most likely Harmonized System (HS) codes at the 4-digit or 6-digit heading level.

Rules:
- Return ONLY a JSON array of objects with ""code"" and ""description"" fields
- Each code should be a valid HS code (4 or 6 digits, e.g. ""8471"" or ""847130"")
- Return up to 5 most likely codes, ordered by confidence
- Do not include any text outside the JSON array
- Format codes without dots or spaces

Example response:
[{""code"":""8471"",""description"":""Automatic data processing machines""},{""code"":""8473"",""description"":""Parts and accessories for office machines""}]";

                var body = new
                {
                    model,
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = $"Classify this product: {keyword}" }
                    },
                    temperature = 0.1,
                    max_tokens = 512
                };

                req.AddJsonBody(body);
                var resp = await client.ExecuteAsync(req);

                if (!resp.IsSuccessful || string.IsNullOrEmpty(resp.Content))
                    return results;

                var json = JObject.Parse(resp.Content);
                var content = json["choices"]?[0]?["message"]?["content"]?.ToString();
                if (string.IsNullOrEmpty(content))
                    return results;

                // Extract JSON array from response (LLM may wrap it in markdown code blocks)
                var arrayMatch = Regex.Match(content, @"\[.*\]", RegexOptions.Singleline);
                if (!arrayMatch.Success)
                    return results;

                var predictions = JArray.Parse(arrayMatch.Value);

                for (var i = 0; i < predictions.Count; i++)
                {
                    var code = predictions[i]["code"]?.ToString();
                    var desc = predictions[i]["description"]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(code))
                        continue;

                    // Look up the code in the database
                    var hsCodes = await _hsCodeService.GetWithHSCodeOptionsAsync(code, null, null);
                    if (hsCodes.Count == 0)
                    {
                        // Try partial match — search children with this prefix
                        hsCodes = await _hsCodeService.GetWithHSCodeOptionsAsync(null, code, null);
                    }

                    var foundInDb = hsCodes.Count > 0;

                    // Log every Groq prediction for accuracy tracking (fire-and-forget)
                    _ = _searchlogService.CreateAsync(new SearchLog
                    {
                        Keyword = keyword,
                        Prediction = $"{code}-{desc}",
                        Rating = i + 1,
                        Threshold = 0,
                        Source = "groq",
                        FoundInDb = foundInDb,
                        Created = DateTime.Now
                    });

                    if (!foundInDb)
                        continue;

                    var conf = Math.Max(0.45f, 0.75f - i * 0.06f);
                    var rm = new BLResultModel
                    {
                        Prediction = desc.Length > 0 ? desc : keyword,
                        Code = code,
                        Rating = conf,
                        Tags = new List<string> { "ai" },
                        HSCodes = hsCodes,
                        PHSCodes = new List<HSCode>()
                    };

                    // Build ancestor chain
                    var ancestors = new List<HSCode>();
                    var cur = hsCodes.FirstOrDefault();
                    while (cur != null && !string.IsNullOrEmpty(cur.ParentId))
                    {
                        var parents = await _hsCodeService.GetWithOptionsAsync(cur.ParentId, null, null);
                        var parent = parents.FirstOrDefault();
                        if (parent != null)
                        {
                            ancestors.Insert(0, parent);
                            cur = parent;
                        }
                        else break;
                    }
                    rm.PHSCodes = ancestors;
                    results.Add(rm);
                }
            }
            catch
            {
                // Groq API not available — return empty
            }

            return results;
        }

        public Dictionary<string, float> GetHSCode(string product, double threshold)
        {
            ModelInput data = new ModelInput
            {
                Keyword = product
            };
            // Make a single prediction on the sample data and print results
            Dictionary<string, float> predictionResult = ConsumeModel.Predict(data, threshold);
            //Dictionary<string, float> pResult
            var i = 0;
            foreach (var result in predictionResult.ToList())
            {
                if (i < 1)
                {
                    var log = new SearchLog { Keyword = product, Threshold = threshold, Prediction = result.Key, Rating = result.Value, Created = DateTime.Now };
                    _searchlogService.CreateAsync(log);
                }
                i++;
                if (threshold > result.Value) predictionResult.Remove(result.Key);
            }

            return predictionResult;
        }


    }
}

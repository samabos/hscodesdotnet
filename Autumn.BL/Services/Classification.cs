using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.BL.Models.Request.V3;
using Autumn.BL.Models.Response.V3;
using Autumn.Domain.Models;
using Autumn.Service.Interface;
using Autumn_UIML.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
                BLSearchResponse response = new BLSearchResponse { Success = true };
                BLResultModel rm = new BLResultModel();
                var rms = new List<BLResultModel>();
                var records = new Dictionary<string, List<BLResultModel>>();

                //Do Navigation or Tag Query
                rm.Prediction = string.Empty;// item.Key;
                rm.Code = request.pid;// aiarr[1];
                rm.Rating = 0;// item.Value;
                rm.Tags = new List<string>();
                rm.PHSCodes = new List<HSCode>();
                rm.HSCodes = new List<HSCode>();





                //Naigation Logic this should be seperated

                if (!string.IsNullOrEmpty(request.settings))
                {
                    return await Navigation(request, response, rm, rms, records);
                }
                else
                {
                    // Check if there is a direct match from the database
                    List<Product> products = await _productService.GetByKeywordAsync(request.keyword);

                    var ctn = products.Count(x => x.Tags != null);


                    if (products.Count > 0)
                    {
                        foreach (var product in products)
                        {
                            rm = await LoadProduct(rms, product);
                            if (ctn == 0)
                            {
                                records.Add("match", rms);
                                return new BLSearchResponse { Success = true, Records = records };
                            }
                        }
                        records.Add("match", rms);
                        return new BLSearchResponse { Success = true, Records = records };

                    }
                    else if (products.Count == 0)
                    {
                        // there is no direct match from the database Attempt Synonyms
                        var synonyms = await GetMatchSynonyms(request.keyword);
                        if (synonyms.Count > 0)
                        {
                            foreach (var product in synonyms)
                            {
                                rm = await LoadProduct(rms, product);
                            }
                            records.Add("synonym", rms);
                            response.Records = records;
                            return response;

                        }
                        else
                        {

                            //Attempt Prediction
                            rm = await AIMethod(request, response, rms);
                            records.Add("ai", rms);
                            response.Records = records;
                            return response;
                        }
                    }
                    response.Records = records;
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new BLSearchResponse { Success = false, Error = new[] { ex.Message } };
            }
        }

        private async Task<BLResultModel> LoadProduct(List<BLResultModel> rms, Product product)
        {
            BLResultModel rm = new BLResultModel();
            rm.Tags = new List<string>();
            //   var aiarr = product.Key.Split('-');
            rm.HSCodes = await _hsCodeService.GetWithHSCodeOptionsAsync(product.Code, null, null);
            //rm.HSCodes = Result2;
            rm.Prediction = product.Keyword;
            rm.Code = product.Code;
            if (product.Tags != null)
                rm.Tags.AddRange(product.Tags);
            //rm.Rating = item.Value;
            rm.PHSCodes = await _hsCodeService.GetWithOptionsAsync(rm.HSCodes.FirstOrDefault().ParentId, null, null);
            rms.Add(rm);
            return rm;
        }

        private async Task<List<Product>> GetMatchSynonyms(string keyword)
        {
            var collector = new List<Product>();
            var synonyms = await GetSynonyms(keyword.ToLower());
            foreach (var synonym in synonyms.ToList())
            {
                List<Product> productExist = await _productService.GetLikeKeywordAsync(synonym);
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
                return synonyms.Synonyms;
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
                //rm.HSCodes = Result2;
                //rm.Code = aiarr[1];
                //rm.Rating = item.Value;
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

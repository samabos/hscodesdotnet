using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.API.Contract.V1;
using Autumn.API.Contract.V1.Responses;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using Autumn.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Autumn.API.V1
{
    [Authorize]
    //[Route("api/[controller]")]
    [ApiController]
    public class KeywordsController : ControllerBase
    {

        private readonly KeywordService _keywordService;
        private readonly ProductService _productService;

        public KeywordsController(KeywordService keywordService, ProductService productService)
        {
            _keywordService = keywordService;
            _productService = productService;
        }

        [HttpGet(ApiRoutes.Keyword.Get)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var products = await _productService.GetAsync();
                var keywords = await _keywordService.GetAsync();
                var remaining = products.Where(x=> !keywords.Select(s=>s.ParentKeyword).Contains(x.Keyword)).OrderBy(a=>a.Id);

                foreach (var r in remaining)
                {
                    try
                    {
                        if (r.Keyword.Split().Count() < 10)
                        {
                            var client = new RestClient("https://uscensus.prod.3ceonline.com/ui/autocomplete");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddParameter("application/json", "{\"query\":\"" + r.Keyword + "\"}", ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);

                            var rep = JsonConvert.DeserializeObject<KeywordAPIResponsecs>(response.Content);
                            //Console.WriteLine(response.Content);
                            if (rep != null)
                            {
                                foreach (var term in rep.results)
                                {
                                    _keywordService.Create(new Keyword { ParentKeyword = r.Keyword, ChildKeyword = term.term });
                                }
                            }

                        }
                    }
                    catch { }
                }
                return Ok(new KeywordResponse { Success = true, Error = new[] { "ok" } });
            }
            catch (Exception ex)
            {
                return BadRequest(new KeywordResponse { Success = false, Error = new[] { ex.Message } });
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Autumn.API.Contract.V2;
using Autumn.API.Contract.V2.Requests;
using Autumn.API.Contract.V2.Responses;
using Autumn.BL.Interface.V2;
using Autumn.BL.Models.Request.V2;
using Autumn.Domain.Infra;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn_UIML.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Autumn.API.V2
{
    [Authorize]
    // [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly HSCodeService _hscodeService;
        private readonly IPredict _predict;
        private readonly ProductService _productService;
        private IConfiguration _configuration;
        private readonly IClassification _classification;
        private readonly IMapper _mapper;

        public SearchController(IConfiguration configuration, HSCodeService hscodeService, IPredict predict, ProductService productService, IClassification classification, IMapper mapper)
        {
            _hscodeService = hscodeService;
            _predict = predict;
            _productService = productService;
            _configuration = configuration;
            _classification = classification;
            _mapper = mapper;
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        /*[HttpGet(ApiRoutes.Search.Get)]
        public async Task<IActionResult> GetAsync([FromQuery] SearchRequest request)
        {
            try
            {
                SearchResponse response = new SearchResponse { Success = true };
                ResultModel rm = new ResultModel();
                var records = new List<ResultModel>();

                //Do Navigation or Tag Query
                rm.Prediction = string.Empty;// item.Key;
                rm.Code = request.pid;// aiarr[1];
                rm.Rating = 0;// item.Value;
                rm.Tags = new List<string>();
                rm.PHSCodes = new List<HSCode>();
                rm.HSCodes = new List<HSCode>();

                if (!string.IsNullOrEmpty(request.settings))
                {
                    if (request.settings == "nav")
                    {
                        rm.HSCodes = await _hscodeService.GetWithOptionsAsync(request.id, request.pid, request.level);
                        if (!string.IsNullOrEmpty(request.pid))
                            rm.PHSCodes = await _hscodeService.GetWithOptionsAsync(request.pid, null, null);

                    }
                    else if (request.settings == "tag")
                    {
                        rm.HSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(request.id, request.pid, request.level);
                        if (!string.IsNullOrEmpty(request.pid))
                            rm.PHSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(request.pid, null, null);
                    }

                    records.Add(rm);

                    response.Records = records;
                    return Ok(response);
                }
                else
                {
                    List<Product> products = await _productService.GetByKeywordAsync(request.keyword);

                    var ctn = products.Count(x => x.Tags != null);

                    if (products.Count > 0)
                    {
                        foreach (var product in products)
                        {
                            rm = new ResultModel();
                            rm.Tags = new List<string>();
                            //   var aiarr = product.Key.Split('-');
                            rm.HSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(product.Code, null, null);
                            //rm.HSCodes = Result2;
                            rm.Prediction = product.Keyword;
                            rm.Code = product.Code;
                            if (product.Tags != null)
                                rm.Tags.AddRange(product.Tags);
                            //rm.Rating = item.Value;
                            rm.PHSCodes = await _hscodeService.GetWithOptionsAsync(rm.HSCodes.FirstOrDefault().ParentId, null, null);
                            records.Add(rm);
                            if (ctn == 0) return Ok(new SearchResponse { Success = true, Records = records });
                        }

                    }
                    else if (products.Count == 0)
                    {
                        var ai = GetHSCode(request.keyword, double.Parse(_configuration["SiteSettings:Threshold"]));
                        if (ai.Count > 0) response.ai = true;

                        rm = new ResultModel();
                        rm.HSCodes = new List<HSCode>();
                        foreach (var item in ai)
                        {
                            var aiarr = item.Key.Split('-');
                            rm.HSCodes.AddRange(await _hscodeService.GetWithHSCodeOptionsAsync(aiarr[1], null, null));
                            //rm.HSCodes = Result2;
                            //rm.Code = aiarr[1];
                            //rm.Rating = item.Value;
                        }

                        rm.Prediction = request.keyword;
                        rm.PHSCodes = new List<HSCode>();
                        rm.Tags = new List<string>();
                        records.Add(rm);
                    }
                    response.Records = records;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new SearchResponse { Success = false, Error = new[] { ex.Message } });
            }
        }

        private Dictionary<string, float> GetHSCode(string product, double threshold)
        {
            ModelInput data = new ModelInput
            {
                Keyword = product
            };
            // Make a single prediction on the sample data and print results
            Dictionary<string, float> predictionResult = ConsumeModel.Predict(data, threshold);

            return predictionResult;
        }*/
        [HttpGet(ApiRoutes.Search.Get)]
        public async Task<IActionResult> GetAsync([FromQuery] SearchRequest request)
        {
            
            
                SearchResponse response = new SearchResponse { Success = true };
                ResultModel rm = new ResultModel();
                var records = new List<ResultModel>();
                var resquetMapped = _mapper.Map<BLSearchRequest>(request);
                var resp = await _classification.SearchAsync(resquetMapped);
            if (resp.Success)
                return Ok(resp);
            else
                return BadRequest(resp);
            
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.API.Contract.V1;
using Autumn.API.Contract.V1.Responses;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Autumn.API.V1
{
    [ApiController]
    public class CodeListController : ControllerBase
    {

        private readonly CurrencyService _currencyService;
        private readonly ProductService _productService;

        public CodeListController(CurrencyService currencyService, ProductService productService)
        {
            _currencyService = currencyService;
            _productService = productService;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.CodeList.Currency)]
        public async Task<IActionResult> CurrencyAsync()
        {
            CurrencyResponse response = new CurrencyResponse();
            try
            {
                var currency = await _currencyService.GetAsync();
                response.Records = currency.Select(x => new CurrencyObject { CurrencyCode = x.CurrencyCode, Rate = x.Rate, TimeStamp = x.TimeStamp }).ToList();
                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new CurrencyResponse { Success = false, Error = new[] { ex.Message } });
            }
        }

        [HttpGet(ApiRoutes.CodeList.Tags)]
        public async Task<JsonResult> TagsAsync(string query = null)
        {

            TagsResult tr = new TagsResult();
            try
            {
                List<Product> p = await _productService.GetByTagsAsync(query);
                var tags = p.Select(x => x.Tags);
                tr.Results = new List<Result>();
                foreach (var tagarr in tags)
                {
                    foreach (var tag in tagarr)
                    {

                        if (tr.Results.Any(x => x.Name != tag))
                        {
                            Result r = new Result { Name = tag, Text = tag, Value = tag };
                            tr.Results.Add(r);
                        }
                        else if (tr.Results.Count == 0)
                        {
                            Result r = new Result { Name = tag, Text = tag, Value = tag };
                            tr.Results.Add(r);
                        }
                    }
                }
                tr.Success = true;
                return new JsonResult(tr);
            }
            catch (Exception ex)
            {
                tr.Success = false;
                return new JsonResult(tr);
            }

        }
        [HttpGet(ApiRoutes.CodeList.Products)]
        public async Task<JsonResult> ProductsAsync(string query = null)
        {

            TagsResult tr = new TagsResult();
            try
            {
                List<Product> p = await _productService.GetLikeKeywordAsync(query);
                // var tags = p.Select(x => x.Tags);
                tr.Results = new List<Result>();

                foreach (var tag in p)
                {
                    if (tr.Results.Count == 0)
                    {
                        Result r = new Result { Name = tag.Keyword, Text = tag.Keyword, Value = tag.Code };
                        tr.Results.Add(r);
                    }
                    else
                    {
                        if (tr.Results.Any(x => x.Name != tag.Keyword))
                        {
                            Result r = new Result { Name = tag.Keyword, Text = tag.Keyword, Value = tag.Code };
                            tr.Results.Add(r);
                        }
                    }
                }

                tr.Success = true;
                return new JsonResult(tr);
            }
            catch (Exception ex)
            {
                tr.Success = false;
                return new JsonResult(tr);
            }

        }
    }
}
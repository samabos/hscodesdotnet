using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.API.Contract.V1;
using Autumn.API.Contract.V1.Requests;
using Autumn.API.Contract.V1.Responses;
using Autumn.Domain.Infra;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn_UIML.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Autumn.API.V1
{
    [Authorize]
    // [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly HSCodeService _hscodeService;
        private readonly IPredict _predict;

        public SearchController(HSCodeService hscodeService, IPredict predict)
        {
            _hscodeService = hscodeService;
            _predict = predict;
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Search.Get)]
        public async Task<IActionResult> GetAsync([FromQuery] SearchRequest request)
        {
            try
            {
                List<HSCode> hscodes = new List<HSCode>();
                if (string.IsNullOrEmpty(request.keyword))
                {
                    hscodes = await _hscodeService.GetWithOptionsAsync(request.id, request.pid, request.level);
                }
                else
                {
                    //ProductDesc = productDesc;
                    var ai = _predict.GetHSCode(request.keyword);
                    var aiarr = ai.Prediction.Split('-');
                    hscodes = await _hscodeService.GetWithHSCodeOptionsAsync(null, aiarr[1], null);
                }
                var records = hscodes.Select(x => new HSCodeObject
                {
                    Code = x.Code,
                    Description = x.Description,
                    Id = x.Id,
                    Level = x.Level,
                    Order = x.Order,
                    ParentCode = x.ParentCode,
                    ParentId = x.ParentId,
                    PId = x.PId,
                    SelfExplanatory = x.SelfExplanatory
                }).ToList();

                return Ok(new SearchResponse { Success = true, Records = records });
            }
            catch (Exception ex)
            {
                return BadRequest(new SearchResponse { Success = false, Error = new[] { ex.Message } });
            }
        }
        

    }
}

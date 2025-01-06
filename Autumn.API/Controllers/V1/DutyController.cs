using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autumn.API.Contract.V1;
using Autumn.API.Contract.V1.Requests;
using Autumn.API.Contract.V1.Responses;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Autumn.API.V1
{
    [Authorize]
    [ApiController]
    public class DutyController : ControllerBase
    {
        private readonly CustomsTariffService _tariffService;
        private readonly CurrencyService _currencyService;

        public DutyController(CustomsTariffService tariffService, CurrencyService currencyService)
        {
            _tariffService = tariffService;
            _currencyService = currencyService;
        }


       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Duty.Get)]
        public async Task<IActionResult> GetAsync([FromQuery] DutyRequest request)
        {
            try
            {
                DutyResponse response = new DutyResponse();
                if (ModelState.IsValid)
                {
                    //Get HS Code Tariff 
                    var tariff = _tariffService.GetByHSCode(request.HSCode);
                    var currency = _currencyService.GetByCurrency(request.Currency);
                    var cif = request.Cost + request.Insurance + request.Freight;
                    var cf = request.Cost + request.Freight;
                    response.ExRate = decimal.Parse(currency.Rate);
                    var duty = cif * (decimal.Parse(tariff.DUTY) / 100);
                    var vat = (cif + duty) * (decimal.Parse(tariff.VAT) / 100);
                    var sur = cif * (decimal.Parse(tariff.SUR) / 100);
                    var etl = cif * (decimal.Parse(tariff.ETLS) / 100);
                    var ciss = cif * (decimal.Parse(tariff.CISS) / 100);
                    var nac = cif * (decimal.Parse(tariff.NAC) / 100);
                    var levy = cif * (decimal.Parse(tariff.LEVY) / 100);


                    response.ProductDesc = request.ProductDesc;
                    response.HSCode = request.HSCode;
                    response.Cost = request.Cost;
                    response.Freight = request.Freight;
                    response.Insurance = request.Insurance;
                    response.Currency = request.Currency;

                    response.CF = cf;
                    response.CIF = cif;
                    response.CIFLocal = cif * response.ExRate;
                    response.IDRate = tariff.DUTY;
                    response.IDPayableLocal = duty * response.ExRate;
                    response.VATRate = tariff.VAT;
                    response.VATPayableLocal = vat * response.ExRate;
                    response.ETLRate = tariff.ETLS;
                    response.ETLPayableLocal = etl * response.ExRate;
                    response.SURRate = tariff.SUR;
                    response.SURPayableLocal = sur * response.ExRate;
                    response.CISSRate = tariff.CISS;
                    response.CISSPayableLocal = ciss * response.ExRate;
                    response.NACRate = tariff.NAC;
                    response.NACPayableLocal = nac * response.ExRate;
                    response.LEVYRate = tariff.LEVY;
                    response.LEVYPayableLocal = levy * response.ExRate;
                    response.TotalPayableLocal = (duty + vat + sur + etl + ciss + nac + levy) * response.ExRate;
                    response.HSCodeDescription = tariff.Description;

                    response.Success = true;

                    return Ok(response);
                }
                else {
                    StringBuilder sb = new StringBuilder();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            sb.Append(error.ErrorMessage);
                            sb.AppendLine();
                            sb.Append(error.Exception.Message);
                        }
                    }
                    return BadRequest(new DutyResponse { Success = false, Error = new[] { sb.ToString() } });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new DutyResponse { Success = false, Error = new[] { ex.Message } });
            }
        }
    }
}
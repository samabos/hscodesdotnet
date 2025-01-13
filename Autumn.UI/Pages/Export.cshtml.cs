using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn.Service.Interface;
using Autumn.UI.Contract.V1.Responses;
using Autumn_UIML.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Autumn.UI.Pages
{
    public class ExportModel : PageModel
    {
        private IConfiguration _configuration;
        private HttpClient _client;
        private readonly IHsCodeService _hscodeService;
        public ExportModel(IConfiguration configuration, IHsCodeService hscodeService)
        {
            _configuration = configuration;
            _client = new HttpClient();
            _hscodeService = hscodeService;
        }
        [BindProperty]
        [Required]
        [Display(Name = "Product Description")]
        public string ProductDesc { get; set; }
        public ClassifyCommodityResponse Result { get; set; }
        public List<HSCode> Result2 { get; set; }

        public async Task OnGetAsync(string id = null, string pid = null, string level = null, string productDesc = null)
        {
            ProductDesc = productDesc;
            Result2 = await _hscodeService.GetWithOptionsAsync(id, pid, level);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Result = await GetHSCode(ProductDesc);
                    var ai = GetHSCode(ProductDesc);
                    var aiarr = ai.Prediction.Split('-');
                    Result2 = await _hscodeService.GetWithHSCodeOptionsAsync(null, aiarr[1], null);
                }
                catch { }
            }
            return Page();
        }

        //private async Task<ClassifyCommodityResponse> GetHSCode(string product)
        //{
        //   // AuthenticationResult token = await GetToken();
        //    ClassifyCommodityResponse response = new ClassifyCommodityResponse();
        //    string m = JsonConvert.SerializeObject(new ClassifyCommodityRequest
        //    {
        //        ItemDescription = product,
        //    });
        //    var content = new StringContent(m, Encoding.UTF8, "application/json");
        //    var url = $"{_configuration.GetValue<string>("SiteSettings:APIURL")}/classify/commodity";
        //    HttpResponseMessage payload = new HttpResponseMessage();
        //    payload = await _client.PostAsync(url, content);
        //    var result = await payload.Content.ReadAsStringAsync();
        //    response = JsonConvert.DeserializeObject<ClassifyCommodityResponse>(result);
        //    if (payload.IsSuccessStatusCode) response.Success = true;
        //    else response.Success = false;

        //    return response;
        //}
        private ModelOutput GetHSCode(string product)
        {
            ModelInput data = new ModelInput
            {
                Keyword = product
            };
            // Make a single prediction on the sample data and print results
            ModelOutput predictionResult = ConsumeModel.Predict(data);

            return predictionResult;
        }

    }
}
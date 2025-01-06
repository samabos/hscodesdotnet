using System.ComponentModel.DataAnnotations;
using Autumn.UI.Contract.V1.Responses;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using AutoMapper;
using Autumn.BL.Interface.V3;

namespace Autumn.UI.Pages
{
    public class ImportModel : PageModel
    {
        private IConfiguration _configuration;
        private HttpClient _client;
        private readonly HSCodeService _hscodeService;
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        private readonly IClassification _classification;

        public ImportModel(IConfiguration configuration, HSCodeService hscodeService, ProductService productService
            , IMapper mapper
            , IClassification classification)
        {
            _configuration = configuration;
            _client = new HttpClient();
            _hscodeService = hscodeService;
            _productService = productService;
            IModel = new List<ResultModel>();
            AI = false;
            _mapper = mapper;
            _classification = classification;
        }
        public ClassifyCommodityResponse Result { get; set; }
        public List<HSCode> Result2 { get; set; }


        [BindProperty]
        [Required]
        [Display(Name = "Product Description")]
        public string ProductDesc { get; set; }
        public List<ResultModel> IModel { get; set; }
        public bool AI { get; set; }

        public async Task OnGetAsync(string id = null, string pid = null, string level = null, string productDesc = null, string settings = "nav")
        {
            ResultModel rm = new ResultModel();
            ProductDesc = productDesc;
            rm = new ResultModel();
            rm.Prediction = string.Empty;// item.Key;
            rm.Code = pid;// aiarr[1];
            rm.Rating = 0;// item.Value;
            rm.Tags = new List<string>();
            rm.PHSCodes = new List<HSCode>();
            rm.HSCodes = new List<HSCode>();

            if (settings == "nav")
            {
                rm.HSCodes = await _hscodeService.GetWithOptionsAsync(id, pid, level);
                if (!string.IsNullOrEmpty(pid))
                    rm.PHSCodes = await _hscodeService.GetWithOptionsAsync(pid, null, null);

            }
            else if (settings == "tag")
            {
                rm.HSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(id, pid, level);
                if (!string.IsNullOrEmpty(pid))
                    rm.PHSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(pid, null, null);
            }

            IModel.Add(rm);
            //GetBYTagInput = 
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ResultModel rm = new ResultModel();

            if (ModelState.IsValid)
            {
                IModel = new List<ResultModel>();
                try
                {
                    List<Product> products = await _productService.GetByKeywordAsync(ProductDesc);

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
                            IModel.Add(rm);
                            if (ctn == 0) return Page();
                        }

                    }
                    else if (products.Count == 0)
                    {
                        var ai = _classification.GetHSCode(ProductDesc, double.Parse(_configuration["SiteSettings:Threshold"]));
                        if (ai.Count > 0) AI = true;

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

                        rm.Prediction = ProductDesc;
                        rm.PHSCodes = new List<HSCode>();
                        rm.Tags = new List<string>();
                        IModel.Add(rm);
                    }
                    //else if (products.Count == 1)
                    //{
                    //    rm = new ResultModel();
                    //    rm.Tags = new List<string>();
                    //    rm.PHSCodes = new List<HSCode>();
                    //    var product = products.FirstOrDefault();
                    //    rm.HSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(product.Code, null, null);
                    //    rm.PHSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(rm.HSCodes.FirstOrDefault().ParentCode, null, null);
                    //    rm.Prediction = product.Keyword;
                    //    rm.Code = product.Code;
                    //    rm.Tags = new List<string>();
                    //    IModel.Add(rm);
                    //}

                    //Result = await GetHSCode(ProductDesc);

                }
                catch (Exception ex) { }

                //var request = _mapper.Map<BLSearchRequest>(InputModel);
                //var result = await _classification.SearchAsync(request);
                //foreach () { }
                //var response = _mapper.Map<SearchResponse>(result);
                //ResultModel = response.Records;
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
        //private Dictionary<string, float> GetHSCode(string product, double threshold)
        //{
        //    ModelInput data = new ModelInput
        //    {
        //        Keyword = product
        //    };
        //    // Make a single prediction on the sample data and print results
        //    Dictionary<string, float> predictionResult = ConsumeModel.Predict(data, threshold);

        //    return predictionResult;
        //}

    }
}
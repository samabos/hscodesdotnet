

namespace Autumn.BL.Services.V2 { 

//    public class Classification : IClassification
//    {
//        private readonly HSCodeService _hscodeService;
//        private readonly IPredict _predict;
//        private readonly ProductService _productService;
//        private readonly SearchLogService _searchlogService;
//        private IConfiguration _configuration;

//        public Classification(IConfiguration configuration, HSCodeService hscodeService, IPredict predict, ProductService productService
//            , SearchLogService searchlogService)
//        {
//            _hscodeService = hscodeService;
//            _predict = predict;
//            _productService = productService;
//            _configuration = configuration;
//            _searchlogService = searchlogService;
//        }

//        public async Task<BLSearchResponse> SearchAsync(BLSearchRequest request)
//        {
//            try
//            {
//                BLSearchResponse response = new BLSearchResponse { Success = true };
//                BLResultModel rm = new BLResultModel();
//                var records = new List<BLResultModel>();

//                //Do Navigation or Tag Query
//                rm.Prediction = string.Empty;// item.Key;
//                rm.Code = request.pid;// aiarr[1];
//                rm.Rating = 0;// item.Value;
//                rm.Tags = new List<string>();
//                rm.PHSCodes = new List<HSCode>();
//                rm.HSCodes = new List<HSCode>();

//                if (!string.IsNullOrEmpty(request.settings))
//                {
//                    if (request.settings == "nav")
//                    {
//                        rm.HSCodes = await _hscodeService.GetWithOptionsAsync(request.id, request.pid, request.level);
//                        if (!string.IsNullOrEmpty(request.pid))
//                            rm.PHSCodes = await _hscodeService.GetWithOptionsAsync(request.pid, null, null);

//                    }
//                    else if (request.settings == "tag")
//                    {
//                        rm.HSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(request.id, request.pid, request.level);
//                        if (!string.IsNullOrEmpty(request.pid))
//                            rm.PHSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(request.pid, null, null);
//                    }

//                    records.Add(rm);

//                    response.Records = records;
//                    return response;
//                }
//                else
//                {
//                    List<Product> products = await _productService.GetByKeywordAsync(request.keyword);

//                    var ctn = products.Count(x => x.Tags != null);

//                    if (products.Count > 0)
//                    {
//                        foreach (var product in products)
//                        {
//                            rm = new BLResultModel();
//                            rm.Tags = new List<string>();
//                            //   var aiarr = product.Key.Split('-');
//                            rm.HSCodes = await _hscodeService.GetWithHSCodeOptionsAsync(product.Code, null, null);
//                            //rm.HSCodes = Result2;
//                            rm.Prediction = product.Keyword;
//                            rm.Code = product.Code;
//                            if (product.Tags != null)
//                                rm.Tags.AddRange(product.Tags);
//                            //rm.Rating = item.Value;
//                            rm.PHSCodes = await _hscodeService.GetWithOptionsAsync(rm.HSCodes.FirstOrDefault().ParentId, null, null);
//                            records.Add(rm);
//                            if (ctn == 0) return new BLSearchResponse { Success = true, Records = records };
//                        }

//                    }
//                    else if (products.Count == 0)
//                    {
//                        var ai = GetHSCode(request.keyword, double.Parse(_configuration["SiteSettings:Threshold"]));
//                        if (ai.Count > 0) response.ai = true;

//                        rm = new BLResultModel();
//                        rm.HSCodes = new List<HSCode>();
//                        foreach (var item in ai)
//                        {
//                            var aiarr = item.Key.Split('-');
//                            rm.HSCodes.AddRange(await _hscodeService.GetWithHSCodeOptionsAsync(aiarr[1], null, null));
//                            //rm.HSCodes = Result2;
//                            //rm.Code = aiarr[1];
//                            //rm.Rating = item.Value;
//                        }

//                        rm.Prediction = request.keyword;
//                        rm.PHSCodes = new List<HSCode>();
//                        rm.Tags = new List<string>();
//                        records.Add(rm);
//                    }
//                    response.Records = records;
//                    return response;
//                }
//            }
//            catch (Exception ex)
//            {
//                return new BLSearchResponse { Success = false, Error = new[] { ex.Message } };
//            }
//        }

//        private Dictionary<string, float> GetHSCode(string product, double threshold)
//        {
//            ModelInput data = new ModelInput
//            {
//                Keyword = product
//            };
//            // Make a single prediction on the sample data and print results
//            Dictionary<string, float> predictionResult = ConsumeModel.Predict(data, threshold);
//            //Dictionary<string, float> pResult
//            var i = 0;
//            foreach (var result in predictionResult.ToList())
//            {
//                if (i < 1)
//                {
//                    var log = new SearchLog { Keyword = product, Threshold = threshold, Prediction = result.Key, Rating = result.Value, Created = DateTime.Now };
//                    _searchlogService.Create(log);
//                }
//                i++;
//                if (threshold > result.Value) predictionResult.Remove(result.Key);
//            }

//            return predictionResult;
//        }
//    }
}

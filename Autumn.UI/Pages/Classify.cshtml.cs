
using AutoMapper;
using Autumn.BL.Models.Request.V3;
using Autumn.Service.Interface;
using Autumn.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Autumn.UI.Pages
{
    public class ClassifyModel : PageModel
    {
        private readonly IMapper _mapper;
        private IConfiguration _configuration;
        private HttpClient _client;
        private IClassification _classification;

        public ClassifyModel(IMapper mapper, IConfiguration configuration,IClassification classification) {

            _mapper = mapper;
            _configuration = configuration;
            _client = new HttpClient();
            _classification = classification;
            ResultModel = new Dictionary<string, List<ResultModel>>();
        }

        public Dictionary<string, List<ResultModel>> ResultModel { get; set; }

        [BindProperty]
        public InputModel InputModel { get; set; }


        public async Task OnGetAsync()
        {
            //var request = _mapper.Map<BLSearchRequest>(InputModel);
            //var result = await _classification.SearchAsync(request);
            //ResultModel = _mapper.Map<List<ResultModel>>(result.Records);
        }

        public async Task OnPostAsync()
        {
            var request = _mapper.Map<BLSearchRequest>(InputModel);
            var result = await _classification.SearchAsync(request);
            //foreach () { }
            var response = _mapper.Map<SearchResponse>(result);
            ResultModel = response.Records;
        }

        public async Task<PartialViewResult> OnGetPartialAsync(string id = null, string pid = null, string level = null, string keyword = null, string settings = "nav") {
            InputModel InputModel = new InputModel
            {
                id = id,
                pid = pid,
                level = level,
                keyword = keyword,
                settings = settings
            };
            var request = _mapper.Map<BLSearchRequest>(InputModel);
            var result = await _classification.SearchAsync(request);
            //foreach () { }
            var response = _mapper.Map<SearchResponse>(result);
            //ResultModel = response.Records;
            //return Partial("_NavHSCode");
            return new PartialViewResult
            {
                ViewName = "_NavHSCode",
                ViewData = new ViewDataDictionary<Dictionary<string, List<ResultModel>>>(ViewData, response.Records)
            };
        }
    }
}

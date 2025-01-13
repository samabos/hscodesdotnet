using Microsoft.AspNetCore.Mvc.RazorPages;
using Autumn.Domain.Services;
using Autumn.Domain.Models;
using Autumn.Domain.Infra;
using Autumn.Service.Interface;

namespace Autumn.UI.Pages
{
    public class LoadModel : PageModel
    {
        private readonly Autumn.Domain.Data.classificationContext _context;

        private readonly IHsCodeService _hscodeService;
        private readonly IProductService _productService;
        private readonly ProductService2 _productService2;
        private readonly DocumentService _documentService;
        private readonly HSCodeToDocumentService _hscodeToDocumentService;
        private readonly CurrencyService _curencyService;
        private readonly IExRate _exRate;
        private readonly ITokenizer _tokenizer;

        public LoadModel(Autumn.Domain.Data.classificationContext context, IHsCodeService hscodeService,
            IProductService productService, ProductService2 productService2, DocumentService documentService,
            HSCodeToDocumentService hscodeToDocumentService, CurrencyService curencyService,
            IExRate exRate,
            ITokenizer tokenizer)
        {
            _context = context;
            _hscodeService = hscodeService;
            _productService = productService;
            _productService2 = productService2;
            _documentService = documentService;
            _hscodeToDocumentService = hscodeToDocumentService;
            _curencyService = curencyService;
            _exRate = exRate;
            _tokenizer = tokenizer;
        }

        public IList<HSCode> Hscodes { get;set; }

        public async Task OnGetAsync()
        {

    /*        
                //var products = await _productService.GetAsync();
                //foreach (var product in products)
                //{
                //    if (product.Tags != null)
                //    {
                //        if (product.Tags.Contains("HS Code"))
                //            _productService.Remove(product);
                //        if (product.Tags.Contains("HS Description"))
                //            _productService.Remove(product);
                //    }
                //}

!*/
            //try
            //{

            //    ///Tokenization of HScode Description/////
            //    var hscodes = await _hscodeService.GetAsync();
            //    foreach (var item in hscodes)
            //    {
            //        if (item.Code.Length < 7) continue;
            //        IList<string> tokens = _tokenizer.GetTokens(item.Description);
            //        if (tokens != null)
            //        {
            //            foreach (var token in tokens)
            //            {
            //                var p = new Product();
            //                p.Code = item.Code;
            //                p.Class = "Code";
            //                p.Keyword = token;
            //                p.Tags = new[] { item.SelfExplanatory };

            //                //delete all redundancy
            //                var dps = await _productService.GetByTagsAsync(item.SelfExplanatory);
            //                if (dps != null)
            //                {
            //                    foreach (var dp in dps.ToList())
            //                    {
            //                        if(dp != null) {
            //                            if (dp.Keyword.Equals(token))
            //                            {
            //                                _productService.Remove(dp);
            //                            }

            //                        }
            //                    }

            //                }
            //                _productService.Create(p);

            //                //p = new Product2();
            //                //p.Code = item.Code;
            //                //p.Class = "Code";
            //                //p.Keyword = item.Code;
            //                //p.Tags = new[] { "HS Code" };
            //                //_productService2.Create(p);
            //            }
            //        }
            //    }


            //}
            //catch (Exception ex) {

            //}




            //Load Products from MongoDb to MSSQL
            //var productsMongo = await _productService.GetAsync();
            //foreach(var item in productsMongo){
            //    _context.Products.Add(new Products {Class=item.Class,Code=item.Code,Keyword=item. });
            //}


            //Load Curency 2020 Data from CBN

            //var rates = _exRate.Load();
            //var curencies = rates.Select(x => new Currency
            //{
            //    CurrencyCode = x.Currency,
            //    Rate = x.CentralRate,
            //    TimeStamp = DateTime.Now.Date.ToString()
            //}).Take(12).ToList();
            //if (curencies.Count > 0)
            //{
            //    var existC = _curencyService.Get();

            //    foreach (var ec in existC)
            //    {
            //        _curencyService.Remove(ec);
            //    }
            //    foreach (var c in curencies)
            //    {
            //        _curencyService.Create(c);
            //    }
            //}

            //Load HSCode 2017 Data
            //Hscodes = await _context.Hscode.Select(x => new Models.HSCode
            //{
            //    Code = x.Code,
            //    Description = x.Description,
            //    Id = x.Id,
            //    Level = (int)x.Level,
            //    Order = (int)x.Order,
            //    ParentCode = x.Parent,
            //    ParentId = x.ParentId,
            //    SelfExplanatory = x.SelfExplanatoryEnglish
            //}).ToListAsync();
            //foreach (var h in Hscodes)
            //{
            //    _hscodeService.Create(h);
            //}

            // Load Keywords data
            //var keywords = await _context.Keyworddata.Select(x => new Models.Product
            //{
            //    Code = x.Code,
            //    Class = x.Class,
            //    Keyword = x.Keyword
            //}).ToListAsync();
            //foreach (var h in keywords)
            //{
            //    _productService.Create(h);
            //}
            //Load Kewords Of Animals

            // var client = new RestClient("https://api.semantic-ui.com/tags/");
            // client.Timeout = -1;
            // var request = new RestRequest(Method.GET);
            // request.AddHeader("Content-Type", "application/json");
            //// request.AddParameter("application/json", "{\"query\":\"" + r.Keyword + "\"}", ParameterType.RequestBody);
            // IRestResponse response = client.Execute(request);

            // var rep = JsonConvert.DeserializeObject<TagsResult>(response.Content);
            // //Console.WriteLine(response.Content);
            // if (rep != null)
            // {
            //     foreach (var term in rep.Results)
            //     {
            //        var p= new Product { Class="Chapter",Code="01",Tags=new string[] { "Live Animals" },Keyword=term.Name};
            //         _productService.Create(p);
            //     }
            // }

            ////Load document data
            //var rows = await _context.Document.Select(x => new Models.Document
            //{
            //   ApplicationForm = x.ApplicationForm,
            //   Code = x.Code,
            //   Country = x.Country,
            //   Description = x.Description,
            //   DurationForIssue = x.DurationForIssue,
            //   InspectionFee = x.InspectionFee,
            //   Issuer = x.Issuer,
            //   LateRenewal = x.LateRenewal,
            //   Level = x.Level,
            //   Parent = x.Parent,
            //   PermitNew = x.PermitNew,
            //   PermitRenewal = x.PermitRenewal,
            //   PnsupportingDocument = x.PnsupportingDocument,
            //   PrsupportingDocument =x.PrsupportingDocument,
            //   Remark = x.Remark,
            //   Validity = x.Validity
            //}).ToListAsync();
            //foreach (var h in rows)
            //{
            //    _documentService.Create(h);
            //}

            ////Load HSCodeToDocument data
            //var rows2 = await _context.HscodeToDocument.Select(x => new Models.HSCodeToDocument
            //{
            //    Agency = x.Agency,
            //    Country = x.Country,
            //    Description = x.Description,
            //    ExpGeneral = x.ExpGeneral,
            //    Hscode = x.Hscode,
            //    HscodeLocal = x.HscodeLocal,
            //    ImpBulkConsignments = x.ImpBulkConsignments,
            //    ImpChemicalsOrRawMaterials = x.ImpChemicalsOrRawMaterials,
            //    ImpFinishedProductsInRetailPack = x.ImpFinishedProductsInRetailPack,
            //    ImpGeneral = x.ImpGeneral,
            //    ImpSupermktOrRestaurant = x.ImpSupermktOrRestaurant
            //}).ToListAsync();
            //foreach (var h in rows2)
            //{
            //    _hscodeToDocumentService.Create(h);
            //}
        }
    }
}

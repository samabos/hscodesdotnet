using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages
{
    public class NoteSUIModel : PageModel
    {
        private readonly HSCodeService _hscodeService;
        private readonly DocumentService _documentService;
        private readonly HSCodeToDocumentService _hscodeToDocumentService;
        private readonly CustomsTariffService _customsTariffService;

        public NoteSUIModel(HSCodeService hscodeService, DocumentService documentService, HSCodeToDocumentService hscodeToDocumentService, CustomsTariffService customsTariffService) {
            _hscodeService = hscodeService;
            _documentService = documentService;
            _hscodeToDocumentService = hscodeToDocumentService;
            _customsTariffService = customsTariffService;
        }

        public List<Document> Document { get; set; }
        public List<HSCodeToDocument> HSCodeToDocument { get; set; }
        public List<HSCode> HSCode { get; set; }
        public List<CustomsTariff> Tariff { get; set; }

        public async Task OnGetAsync(string hscode)
        {
            var documentTask = _documentService.GetAsync();
            var hscodeTask  = _hscodeService.GetWithHSCodeOptionsAsync(hscode, null, null);
            var hsdocsTask = _hscodeToDocumentService.GetWithCodeAsync(hscode);
            var tariff = _customsTariffService.GetByHeaderAsync(hscode);

            HSCode = await hscodeTask;
            HSCodeToDocument = await hsdocsTask;
            Document = await documentTask;
            Tariff = await tariff;
        }
    }
}
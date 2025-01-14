using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages
{
    public class NoteModel : PageModel
    {
        private readonly IHsCodeService _hscodeService;
        private readonly IDocumentService _documentService;
        private readonly IHsCodeDocumentService _hscodeToDocumentService;
        private readonly ICustomsTariffService _customsTariffService;

        public NoteModel(IHsCodeService hscodeService, IDocumentService documentService, IHsCodeDocumentService hscodeToDocumentService, ICustomsTariffService customsTariffService) {
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
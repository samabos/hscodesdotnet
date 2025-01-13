using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages
{
    public class NoteS2Model : PageModel
    {
        private readonly IHsCodeService _hscodeService;
        //private readonly DocumentService _documentService;
        //private readonly HSCodeToDocumentService _hscodeToDocumentService;
        private readonly RequirementService _requirementService;
        private readonly ICustomsTariffService _customsTariffService;

        public NoteS2Model(IHsCodeService hscodeService, RequirementService requirementService, ICustomsTariffService customsTariffService)
        {
            _hscodeService = hscodeService;
            //_documentService = documentService;
            //_hscodeToDocumentService = hscodeToDocumentService;
            _requirementService = requirementService;
            _customsTariffService = customsTariffService;
        }

        //public List<Document> Document { get; set; }
        public List<Requirement> Requirements { get; set; }
        public List<HSCode> HSCode { get; set; }
        public List<CustomsTariff> Tariff { get; set; }

        public async Task OnGetAsync(string hscode)
        {
            //var documentTask = _documentService.GetAsync();
            var hscodeTask = _hscodeService.GetWithHSCodeOptionsAsync(hscode, null, null);
            var requirmentTask = _requirementService.GetByHSCodeAsync(hscode);
            var tariff = _customsTariffService.GetByHeaderAsync(hscode);

            HSCode = await hscodeTask;
            //HSCodeToDocument = await hsdocsTask;
            //Document = await documentTask;
            Tariff = await tariff;
            Requirements = await requirmentTask;

        }
    }
}

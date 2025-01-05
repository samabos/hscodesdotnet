using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.API.Contract.V1;
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
    public class NoteController : ControllerBase
    {

        private readonly HSCodeService _hscodeService;
        private readonly DocumentService _documentService;
        private readonly HSCodeToDocumentService _hscodeToDocumentService;
        private readonly CustomsTariffService _customsTariffService;

        public NoteController(HSCodeService hscodeService, DocumentService documentService, HSCodeToDocumentService hscodeToDocumentService, CustomsTariffService customsTariffService)
        {
            _hscodeService = hscodeService;
            _documentService = documentService;
            _hscodeToDocumentService = hscodeToDocumentService;
            _customsTariffService = customsTariffService;
        }

        [Authorize]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Note.Get)]
        public async Task<IActionResult> GetAsync(string hscode)
        {
            try
            {
                var documentTask = _documentService.GetAsync();
                var hscodeTask = _hscodeService.GetWithHSCodeOptionsAsync(hscode, null, null);
                var hsdocsTask = _hscodeToDocumentService.GetWithCodeAsync(hscode);
                var tariff = _customsTariffService.GetByHeaderAsync(hscode);

                var hscodeList = await hscodeTask;
                var hscodeToDocumentList = await hsdocsTask;
                var documentList = await documentTask;
                var tariffList = await tariff;

                var hscodeObj = hscodeList.Select(x => new HSCodeObject
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

                var hscodeToDocumentObj = hscodeToDocumentList.Select(x => new HscodeToDocumentObject
                {
                    Agency = x.Agency,
                    Country = x.Country,
                    Description = x.Description,
                    ExpGeneral = x.ExpGeneral,
                    Hscode = x.Hscode,
                    HscodeLocal = x.HscodeLocal,
                    Id = x.Id,
                    ImpBulkConsignments = x.ImpBulkConsignments,
                    ImpChemicalsOrRawMaterials = x.ImpChemicalsOrRawMaterials,
                    ImpFinishedProductsInRetailPack = x.ImpFinishedProductsInRetailPack,
                    ImpGeneral = x.ImpGeneral,
                    ImpSupermktOrRestaurant = x.ImpSupermktOrRestaurant
                }).ToList();

                var documentObj = documentList.Select(x => new DocumentObject
                {
                    ApplicationForm = x.ApplicationForm,
                    Code = x.Code,
                    Country = x.Country,
                    Description = x.Description,
                    DurationForIssue = x.DurationForIssue,
                    Id = x.Id,
                    InspectionFee = x.InspectionFee,
                    Issuer = x.Issuer,
                    LateRenewal = x.LateRenewal,
                    Level = x.Level,
                    Parent = x.Parent,
                    PermitNew = x.PermitNew,
                    PermitRenewal = x.PermitRenewal,
                    PnsupportingDocument = x.PnsupportingDocument,
                    PrsupportingDocument = x.PrsupportingDocument,
                    Remark = x.Remark,
                    Validity = x.Validity
                }).ToList();

                var tariffObj = tariffList.Select(x => new CustomsTariffObject
                {
                    CISS = x.CISS,
                    Description = x.Description,
                    DUTY = x.DUTY,
                    ETLS = x.ETLS,
                    Header = x.Header,
                    HSCode = x.HSCode,
                    Id = x.Id,
                    LEVY = x.LEVY,
                    NAC = x.NAC,
                    SUR = x.SUR,
                    VAT = x.VAT
                }).ToList();

                return Ok(new NoteResponse { Success = true, Documents = documentObj, Records = hscodeObj, RecordsToDocuments = hscodeToDocumentObj, Tariff = tariffObj });
            }
            catch (Exception ex)
            {
                return BadRequest(new NoteResponse { Success = false, Error = new[] { ex.Message } });
            }
        }
    }
}
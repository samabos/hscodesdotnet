using Autumn.API.Dto;
using Autumn.Service.Interface;

namespace Autumn.API.Endpoints;

public static class NoteEndpoints
{
    public static void MapNoteEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/note")
            .WithTags("Notes")
            .AllowAnonymous();

        group.MapGet("/{hscode}", GetNote);
    }

    private static async Task<IResult> GetNote(
        IHsCodeService hsCodeService,
        IDocumentService documentService,
        IHsCodeDocumentService hsCodeDocumentService,
        ICustomsTariffService tariffService,
        string hscode,
        string? country = null)
    {
        try
        {
            // Run queries in parallel
            var hscodeTask = hsCodeService.GetWithHSCodeOptionsAsync(hscode, null, null);
            var documentTask = documentService.GetAsync();
            var hsdocsTask = hsCodeDocumentService.GetWithCodeAsync(hscode);
            var tariffTask = tariffService.GetByHeaderAndCountryAsync(hscode, country ?? "NG");

            await Task.WhenAll(hscodeTask, documentTask, hsdocsTask, tariffTask);

            var hscodeList = await hscodeTask;
            var documentList = await documentTask;
            var hscodeToDocumentList = await hsdocsTask;
            var tariffList = await tariffTask;

            return Results.Ok(new NoteApiResponse
            {
                Success = true,
                Records = hscodeList.Select(SearchEndpoints.MapHSCode).ToList(),
                Documents = documentList.Select(x => new DocumentDto
                {
                    Id = x.Id ?? string.Empty,
                    Code = x.Code ?? string.Empty,
                    Description = x.Description ?? string.Empty,
                    Country = x.Country,
                    Issuer = x.Issuer,
                    Level = x.Level?.ToString(),
                    Parent = x.Parent,
                    Validity = x.Validity,
                    DurationForIssue = x.DurationForIssue,
                    ApplicationForm = x.ApplicationForm,
                    InspectionFee = x.InspectionFee,
                    PermitNew = x.PermitNew,
                    PermitRenewal = x.PermitRenewal,
                    LateRenewal = x.LateRenewal,
                    PnsupportingDocument = x.PnsupportingDocument,
                    PrsupportingDocument = x.PrsupportingDocument,
                    Remark = x.Remark
                }).ToList(),
                RecordsToDocuments = hscodeToDocumentList.Select(x => new HSCodeToDocumentDto
                {
                    Id = x.Id ?? string.Empty,
                    Agency = x.Agency,
                    Country = x.Country,
                    Hscode = x.Hscode,
                    HscodeLocal = x.HscodeLocal,
                    Description = x.Description,
                    ImpGeneral = x.ImpGeneral,
                    ImpFinishedProductsInRetailPack = x.ImpFinishedProductsInRetailPack,
                    ImpBulkConsignments = x.ImpBulkConsignments,
                    ImpChemicalsOrRawMaterials = x.ImpChemicalsOrRawMaterials,
                    ImpSupermktOrRestaurant = x.ImpSupermktOrRestaurant,
                    ExpGeneral = x.ExpGeneral
                }).ToList(),
                Tariff = tariffList.Select(x => new CustomsTariffDto
                {
                    Id = x.Id ?? string.Empty,
                    Country = x.Country,
                    Header = x.Header,
                    HSCode = x.HSCode ?? string.Empty,
                    Description = x.Description ?? string.Empty,
                    DUTY = x.DUTY,
                    VAT = x.VAT,
                    LEVY = x.LEVY,
                    NAC = x.NAC,
                    SUR = x.SUR,
                    ETLS = x.ETLS,
                    CISS = x.CISS,
                    NHIL = x.NHIL,
                    GETFUND = x.GETFUND,
                    IDF = x.IDF,
                    RDF = x.RDF
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new NoteApiResponse
            {
                Success = false,
                Error = new[] { ex.Message }
            });
        }
    }
}

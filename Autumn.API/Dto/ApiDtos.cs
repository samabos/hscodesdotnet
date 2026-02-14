namespace Autumn.API.Dto;

// ── Search ──────────────────────────────────────────────────────

public class SearchApiResponse
{
    public bool Success { get; set; }
    public IEnumerable<string>? Error { get; set; }
    public Dictionary<string, List<SearchResultDto>> Records { get; set; } = new();
}

public class SearchResultDto
{
    public List<HSCodeDto> HSCodes { get; set; } = new();
    public List<HSCodeDto> ParentHSCodes { get; set; } = new();
    public string Prediction { get; set; } = string.Empty;
    public float Rating { get; set; }
    public List<string> Tags { get; set; } = new();
    public string Code { get; set; } = string.Empty;
}

public class HSCodeDto
{
    public string Id { get; set; } = string.Empty;
    public string ParentId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string ParentCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? SelfExplanatory { get; set; }
    public long Level { get; set; }
    public long Order { get; set; }
}

// ── Browse ──────────────────────────────────────────────────────

public class BrowseApiResponse
{
    public bool Success { get; set; }
    public IEnumerable<string>? Error { get; set; }
    public List<HSCodeDto> Records { get; set; } = new();
}

// ── Duty Calculator ─────────────────────────────────────────────

public class DutyRequest
{
    public string ProductDesc { get; set; } = string.Empty;
    public string HSCode { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public decimal Freight { get; set; }
    public decimal Insurance { get; set; }
    public string Currency { get; set; } = string.Empty;
}

public class DutyApiResponse
{
    public bool Success { get; set; }
    public IEnumerable<string>? Error { get; set; }

    // Input echo
    public string ProductDesc { get; set; } = string.Empty;
    public string HSCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public decimal Freight { get; set; }
    public decimal Insurance { get; set; }
    public string Currency { get; set; } = string.Empty;

    // Calculated
    public decimal CIF { get; set; }

    // Dynamic rate breakdown (works for any country)
    public List<DutyLineItem> Breakdown { get; set; } = new();
    public decimal TotalDuty { get; set; }

    public string HSCodeDescription { get; set; } = string.Empty;
}

public class DutyLineItem
{
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}

// ── Note ────────────────────────────────────────────────────────

public class NoteApiResponse
{
    public bool Success { get; set; }
    public IEnumerable<string>? Error { get; set; }
    public List<HSCodeDto> Records { get; set; } = new();
    public List<DocumentDto> Documents { get; set; } = new();
    public List<HSCodeToDocumentDto> RecordsToDocuments { get; set; } = new();
    public List<CustomsTariffDto> Tariff { get; set; } = new();
}

public class DocumentDto
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Issuer { get; set; }
    public string? Level { get; set; }
    public string? Parent { get; set; }
    public string? Validity { get; set; }
    public string? DurationForIssue { get; set; }
    public string? ApplicationForm { get; set; }
    public string? InspectionFee { get; set; }
    public string? PermitNew { get; set; }
    public string? PermitRenewal { get; set; }
    public string? LateRenewal { get; set; }
    public string? PnsupportingDocument { get; set; }
    public string? PrsupportingDocument { get; set; }
    public string? Remark { get; set; }
}

public class HSCodeToDocumentDto
{
    public string Id { get; set; } = string.Empty;
    public string? Agency { get; set; }
    public string? Country { get; set; }
    public string? Hscode { get; set; }
    public string? HscodeLocal { get; set; }
    public string? Description { get; set; }
    public string? ImpGeneral { get; set; }
    public string? ImpFinishedProductsInRetailPack { get; set; }
    public string? ImpBulkConsignments { get; set; }
    public string? ImpChemicalsOrRawMaterials { get; set; }
    public string? ImpSupermktOrRestaurant { get; set; }
    public string? ExpGeneral { get; set; }
}

public class CustomsTariffDto
{
    public string Id { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Header { get; set; }
    public string HSCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? DUTY { get; set; }
    public string? VAT { get; set; }
    public string? LEVY { get; set; }
    public string? NAC { get; set; }
    public string? SUR { get; set; }
    public string? ETLS { get; set; }
    public string? CISS { get; set; }
    public string? NHIL { get; set; }
    public string? GETFUND { get; set; }
    public string? IDF { get; set; }
    public string? RDF { get; set; }
}

// ── CodeList ────────────────────────────────────────────────────

public class CurrencyApiResponse
{
    public bool Success { get; set; }
    public IEnumerable<string>? Error { get; set; }
    public List<CurrencyDto> Records { get; set; } = new();
}

public class CurrencyDto
{
    public string CurrencyCode { get; set; } = string.Empty;
    public string Rate { get; set; } = string.Empty;
    public string? TimeStamp { get; set; }
}

public class TagsApiResponse
{
    public bool Success { get; set; }
    public List<TagResult> Results { get; set; } = new();
}

public class TagResult
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

// ── Countries ──────────────────────────────────────────────────

public class CountryApiResponse
{
    public bool Success { get; set; }
    public List<CountryDto> Records { get; set; } = new();
}

public class CountryDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
}

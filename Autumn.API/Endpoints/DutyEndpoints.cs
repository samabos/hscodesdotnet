using Autumn.API.Dto;
using Autumn.Domain.Models;
using Autumn.Service.Interface;

namespace Autumn.API.Endpoints;

public static class DutyEndpoints
{
    private static readonly Dictionary<string, string> RateLabels = new()
    {
        ["DUTY"] = "Import Duty",
        ["VAT"] = "VAT",
        ["LEVY"] = "Levy",
        ["SUR"] = "Surcharge",
        ["ETLS"] = "ETL",
        ["CISS"] = "CISS",
        ["NAC"] = "NAC",
        ["NHIL"] = "NHIL",
        ["GETFUND"] = "GETFund",
        ["IDF"] = "IDF",
        ["RDF"] = "RDF"
    };

    public static void MapDutyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/duty")
            .WithTags("Duty Calculator")
            .AllowAnonymous();

        group.MapGet("/", CalculateDuty);
    }

    private static async Task<IResult> CalculateDuty(
        ICustomsTariffService tariffService,
        string HSCode,
        string Country = "NG",
        string ProductDesc = "",
        decimal Cost = 0,
        decimal Freight = 0,
        decimal Insurance = 0,
        string Currency = "USD")
    {
        try
        {
            // Try exact HSCode match first, then fall back to Header match
            var tariff = await tariffService.GetByHSCodeAndCountryAsync(HSCode, Country);
            if (tariff == null)
            {
                var headerMatches = await tariffService.GetByHeaderAndCountryAsync(HSCode, Country);
                tariff = headerMatches.FirstOrDefault();
            }
            if (tariff == null)
                return Results.BadRequest(new DutyApiResponse
                {
                    Success = false,
                    Error = new[] { $"No tariff found for HS Code: {HSCode} in country: {Country}" }
                });

            var cif = Cost + Insurance + Freight;

            // Build dynamic breakdown from all non-null/non-zero rate fields
            var breakdown = new List<DutyLineItem>();
            decimal dutyAmount = 0; // track import duty for VAT base calculation

            foreach (var (code, rateStr) in GetRateFields(tariff))
            {
                if (!decimal.TryParse(rateStr, out var rate) || rate == 0)
                    continue;

                // VAT is typically calculated on CIF + Import Duty
                var baseAmount = code == "VAT" ? cif + dutyAmount : cif;
                var amount = baseAmount * (rate / 100);

                if (code == "DUTY")
                    dutyAmount = amount;

                breakdown.Add(new DutyLineItem
                {
                    Code = code,
                    Label = RateLabels.GetValueOrDefault(code, code),
                    Rate = rate,
                    Amount = Math.Round(amount, 2)
                });
            }

            return Results.Ok(new DutyApiResponse
            {
                Success = true,
                ProductDesc = ProductDesc,
                HSCode = HSCode,
                Country = Country,
                Cost = Cost,
                Freight = Freight,
                Insurance = Insurance,
                Currency = Currency,
                CIF = cif,
                Breakdown = breakdown,
                TotalDuty = Math.Round(breakdown.Sum(b => b.Amount), 2),
                HSCodeDescription = tariff.Description ?? string.Empty
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new DutyApiResponse
            {
                Success = false,
                Error = new[] { ex.Message }
            });
        }
    }

    private static IEnumerable<(string Code, string? Rate)> GetRateFields(CustomsTariff tariff)
    {
        // Yield DUTY first so its amount is available for VAT base calculation
        yield return ("DUTY", tariff.DUTY);
        yield return ("VAT", tariff.VAT);
        yield return ("LEVY", tariff.LEVY);
        yield return ("SUR", tariff.SUR);
        yield return ("ETLS", tariff.ETLS);
        yield return ("CISS", tariff.CISS);
        yield return ("NAC", tariff.NAC);
        yield return ("NHIL", tariff.NHIL);
        yield return ("GETFUND", tariff.GETFUND);
        yield return ("IDF", tariff.IDF);
        yield return ("RDF", tariff.RDF);
    }
}

using Autumn.API.Dto;
using Autumn.Service.Interface;

namespace Autumn.API.Endpoints;

public static class CodeListEndpoints
{
    public static void MapCodeListEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/codelist")
            .WithTags("Reference Data")
            .AllowAnonymous();

        group.MapGet("/countries", GetCountries);
        group.MapGet("/currency", GetCurrencies);
        group.MapGet("/products/{query?}", GetProducts);
        group.MapGet("/tags/{query?}", GetTags);
    }

    private static async Task<IResult> GetCountries(ICountryService countryService)
    {
        var countries = await countryService.GetAsync();
        return Results.Ok(new CountryApiResponse
        {
            Success = true,
            Records = countries.Select(c => new CountryDto
            {
                Code = c.Code ?? string.Empty,
                Name = c.Name ?? string.Empty,
                Flag = c.Flag ?? string.Empty,
                Currency = c.Currency ?? string.Empty,
                Symbol = c.Symbol ?? string.Empty
            }).ToList()
        });
    }

    private static async Task<IResult> GetCurrencies(ICurrencyService currencyService)
    {
        try
        {
            var currencies = await currencyService.GetAsync();
            return Results.Ok(new CurrencyApiResponse
            {
                Success = true,
                Records = currencies.Select(c => new CurrencyDto
                {
                    CurrencyCode = c.CurrencyCode ?? string.Empty,
                    Rate = c.Rate.ToString(),
                    TimeStamp = c.TimeStamp.ToString("o")
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new CurrencyApiResponse
            {
                Success = false,
                Error = new[] { ex.Message }
            });
        }
    }

    private static async Task<IResult> GetProducts(IProductService productService, string? query = null)
    {
        try
        {
            var products = string.IsNullOrEmpty(query)
                ? await productService.GetAsync()
                : await productService.GetLikeKeywordAsync(query);

            var results = products.Select(p => new TagResult
            {
                Name = p.Keyword ?? string.Empty,
                Value = p.Code ?? string.Empty,
                Text = p.Keyword ?? string.Empty
            }).ToList();

            return Results.Ok(new TagsApiResponse { Success = true, Results = results });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new TagsApiResponse { Success = false });
        }
    }

    private static async Task<IResult> GetTags(IProductService productService, string? query = null)
    {
        try
        {
            var products = string.IsNullOrEmpty(query)
                ? await productService.GetAsync()
                : await productService.GetByTagsAsync(query);

            var results = products.Select(p => new TagResult
            {
                Name = p.Keyword ?? string.Empty,
                Value = p.Code ?? string.Empty,
                Text = p.Keyword ?? string.Empty
            }).ToList();

            return Results.Ok(new TagsApiResponse { Success = true, Results = results });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new TagsApiResponse { Success = false });
        }
    }
}

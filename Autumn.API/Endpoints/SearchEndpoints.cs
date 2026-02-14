using Autumn.API.Dto;
using Autumn.Service.Interface;
using Autumn.BL.Models.Request.V3;

namespace Autumn.API.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/search")
            .WithTags("Search")
            .AllowAnonymous();

        group.MapGet("/", Search);
    }

    private static async Task<IResult> Search(
        IClassification classification,
        string? keyword = null,
        string? id = null,
        string? pid = null,
        string? level = null,
        string? settings = null)
    {
        try
        {
            var request = new BLSearchRequest
            {
                id = id,
                pid = pid,
                level = level,
                keyword = keyword,
                settings = settings
            };

            var resp = await classification.SearchAsync(request);

            if (!resp.Success)
                return Results.BadRequest(new SearchApiResponse
                {
                    Success = false,
                    Error = resp.Error
                });

            // Map BLSearchResponse to clean API response
            var response = new SearchApiResponse { Success = true };

            if (resp.Records != null)
            {
                foreach (var kvp in resp.Records)
                {
                    var results = kvp.Value.Select(r => new SearchResultDto
                    {
                        Prediction = r.Prediction ?? string.Empty,
                        Rating = r.Rating,
                        Code = r.Code ?? string.Empty,
                        Tags = r.Tags ?? new List<string>(),
                        HSCodes = r.HSCodes?.Select(MapHSCode).ToList() ?? new List<HSCodeDto>(),
                        ParentHSCodes = r.PHSCodes?.Select(MapHSCode).ToList() ?? new List<HSCodeDto>()
                    }).ToList();

                    response.Records[kvp.Key] = results;
                }
            }

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new SearchApiResponse
            {
                Success = false,
                Error = new[] { ex.Message }
            });
        }
    }

    internal static HSCodeDto MapHSCode(Autumn.Domain.Models.HSCode x) => new()
    {
        Id = x.Id ?? string.Empty,
        ParentId = x.ParentId ?? string.Empty,
        Code = x.Code ?? string.Empty,
        ParentCode = x.ParentCode ?? string.Empty,
        Description = x.Description ?? string.Empty,
        SelfExplanatory = x.SelfExplanatory,
        Level = x.Level,
        Order = x.Order
    };
}

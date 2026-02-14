using Autumn.API.Dto;
using Autumn.Service.Interface;

namespace Autumn.API.Endpoints;

public static class BrowseEndpoints
{
    public static void MapBrowseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/browse")
            .WithTags("Browse")
            .AllowAnonymous();

        group.MapGet("/", Browse);
    }

    private static async Task<IResult> Browse(
        IHsCodeService hsCodeService,
        string? code = null,
        string? parentCode = null,
        string? parentId = null,
        string? level = null)
    {
        try
        {
            List<Autumn.Domain.Models.HSCode> hscodes;

            if (!string.IsNullOrEmpty(parentId))
            {
                // ID-based navigation (matches Razor page pattern)
                hscodes = await hsCodeService.GetWithOptionsAsync(null, parentId, level);
            }
            else
            {
                hscodes = await hsCodeService.GetWithHSCodeOptionsAsync(code, parentCode, level);
            }

            var records = hscodes.Select(SearchEndpoints.MapHSCode).ToList();

            return Results.Ok(new BrowseApiResponse
            {
                Success = true,
                Records = records
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new BrowseApiResponse
            {
                Success = false,
                Error = new[] { ex.Message }
            });
        }
    }
}

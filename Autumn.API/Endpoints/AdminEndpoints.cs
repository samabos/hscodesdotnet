using Autumn.API.Dto;
using Autumn.Domain.Models;
using Autumn.Service.Interface;

namespace Autumn.API.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/admin")
            .WithTags("Admin")
            .RequireAuthorization();

        // Dashboard
        group.MapGet("/dashboard", GetDashboard);

        // Products CRUD
        group.MapGet("/products", GetProducts);
        group.MapGet("/products/{id}", GetProduct);
        group.MapPost("/products", CreateProduct);
        group.MapPut("/products/{id}", UpdateProduct);
        group.MapDelete("/products/{id}", DeleteProduct);

        // HS Codes CRUD
        group.MapGet("/codes", GetCodes);
        group.MapGet("/codes/{id}", GetCode);
        group.MapPut("/codes/{id}", UpdateCode);

        // Tariffs CRUD
        group.MapGet("/tariffs", GetTariffs);
        group.MapGet("/tariffs/{id}", GetTariff);
        group.MapPost("/tariffs", CreateTariff);
        group.MapPut("/tariffs/{id}", UpdateTariff);
        group.MapDelete("/tariffs/{id}", DeleteTariff);

        // Query Logs
        group.MapGet("/querylogs", GetQueryLogs);
    }

    // ── Dashboard ───────────────────────────────────────────────

    private static async Task<IResult> GetDashboard(
        IProductService productService,
        IHsCodeService hsCodeService,
        ICustomsTariffService tariffService)
    {
        var products = await productService.GetAsync();
        var codes = await hsCodeService.GetAsync();
        var tariffs = await tariffService.GetAsync();

        return Results.Ok(new
        {
            ProductCount = products.Count,
            HSCodeCount = codes.Count,
            TariffCount = tariffs.Count
        });
    }

    // ── Products ────────────────────────────────────────────────

    private static async Task<IResult> GetProducts(IProductService productService)
    {
        var products = await productService.GetAsync();
        return Results.Ok(products);
    }

    private static async Task<IResult> GetProduct(IProductService productService, string id)
    {
        var product = await productService.GetAsync(id);
        return product is null ? Results.NotFound() : Results.Ok(product);
    }

    private static async Task<IResult> CreateProduct(IProductService productService, Product product)
    {
        var created = await productService.CreateAsync(product);
        return Results.Created($"/api/admin/products/{created.Id}", created);
    }

    private static async Task<IResult> UpdateProduct(IProductService productService, string id, Product product)
    {
        await productService.UpdateAsync(id, product);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteProduct(IProductService productService, string id)
    {
        await productService.RemoveAsync(id);
        return Results.NoContent();
    }

    // ── HS Codes ────────────────────────────────────────────────

    private static async Task<IResult> GetCodes(IHsCodeService hsCodeService)
    {
        var codes = await hsCodeService.GetAsync();
        return Results.Ok(codes);
    }

    private static async Task<IResult> GetCode(IHsCodeService hsCodeService, string id)
    {
        var code = await hsCodeService.GetAsync(id);
        return code is null ? Results.NotFound() : Results.Ok(code);
    }

    private static async Task<IResult> UpdateCode(IHsCodeService hsCodeService, string id, HSCode code)
    {
        await hsCodeService.UpdateAsync(id, code);
        return Results.NoContent();
    }

    // ── Tariffs ─────────────────────────────────────────────────

    private static async Task<IResult> GetTariffs(ICustomsTariffService tariffService, string? country = null)
    {
        var tariffs = await tariffService.GetAsync();
        if (!string.IsNullOrEmpty(country))
            tariffs = tariffs.Where(t => t.Country == country || (t.Country == null && country == "NG")).ToList();
        return Results.Ok(tariffs);
    }

    private static async Task<IResult> GetTariff(ICustomsTariffService tariffService, string id)
    {
        var tariff = await tariffService.GetAsync(id);
        return tariff is null ? Results.NotFound() : Results.Ok(tariff);
    }

    private static async Task<IResult> CreateTariff(ICustomsTariffService tariffService, CustomsTariff tariff)
    {
        var created = await tariffService.CreateAsync(tariff);
        return Results.Created($"/api/admin/tariffs/{created.Id}", created);
    }

    private static async Task<IResult> UpdateTariff(ICustomsTariffService tariffService, string id, CustomsTariff tariff)
    {
        await tariffService.UpdateAsync(id, tariff);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteTariff(ICustomsTariffService tariffService, string id)
    {
        await tariffService.RemoveAsync(id);
        return Results.NoContent();
    }

    // ── Query Logs ──────────────────────────────────────────────

    private static async Task<IResult> GetQueryLogs(ISearchLogService searchLogService)
    {
        var logs = await searchLogService.GetAsync();
        return Results.Ok(logs);
    }
}

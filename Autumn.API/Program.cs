using System.Threading.RateLimiting;
using Autumn.API.Endpoints;
using Autumn.Domain.Models;
using Autumn.Infrastructure;
using Autumn.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ── MongoDB & SQL database services ─────────────────────────────
builder.Services.AddDocumentDatabaseServices(configuration);
builder.Services.AddRelationalDatabaseServices(configuration);

// ── Repository & Business services ──────────────────────────────
builder.Services.AddRepositoryServices();
builder.Services.AddBusinessServices();

// ── CORS ────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSPA", policy =>
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:5173", "http://localhost:5174" };

        policy.WithOrigins(origins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ── Rate Limiting ────────────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        RateLimitPartition.GetFixedWindowLimiter(
            ctx.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromSeconds(60),
                QueueLimit = 0
            }));
});

// ── Auth0 JWT Authentication ────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = configuration["Auth0:Domain"];
        options.Audience = configuration["Auth0:Audience"];
    });

builder.Services.AddAuthorization();

// ── OpenAPI / Swagger ───────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "HS Codes API",
        Version = "v1",
        Description = "HS Commodity Classification & Duty Calculator API"
    });
});

var app = builder.Build();

// ── Seed ────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var settings = scope.ServiceProvider.GetRequiredService<IStoreDatabaseSettings>();
    var client = new MongoClient(settings.ConnectionString);
    var db = client.GetDatabase(settings.DatabaseName);

    // Set Country = "NG" on existing tariff records that have no country
    var tariffs = db.GetCollection<CustomsTariff>(settings.CustomsTariffStoreCollectionName);
    var filter = Builders<CustomsTariff>.Filter.Eq(t => t.Country, null);
    var update = Builders<CustomsTariff>.Update.Set(t => t.Country, "NG");
    var result = await tariffs.UpdateManyAsync(filter, update);
    if (result.ModifiedCount > 0)
        app.Logger.LogInformation("Seed: Updated {Count} tariff records with Country = 'NG'", result.ModifiedCount);

    // Seed countries collection if empty
    var countries = db.GetCollection<Country>(settings.CountryStoreCollectionName);
    var countryCount = await countries.CountDocumentsAsync(Builders<Country>.Filter.Empty);
    if (countryCount == 0)
    {
        var seedCountries = new List<Country>
        {
            new() { Code = "NG", Name = "Nigeria", Flag = "\U0001F1F3\U0001F1EC", Currency = "NGN", Symbol = "\u20A6" },
            new() { Code = "GH", Name = "Ghana", Flag = "\U0001F1EC\U0001F1ED", Currency = "GHS", Symbol = "GH\u20B5" },
            new() { Code = "KE", Name = "Kenya", Flag = "\U0001F1F0\U0001F1EA", Currency = "KES", Symbol = "KSh" },
            new() { Code = "ZA", Name = "South Africa", Flag = "\U0001F1FF\U0001F1E6", Currency = "ZAR", Symbol = "R" },
            new() { Code = "GB", Name = "United Kingdom", Flag = "\U0001F1EC\U0001F1E7", Currency = "GBP", Symbol = "\u00A3" },
        };
        await countries.InsertManyAsync(seedCountries);
        app.Logger.LogInformation("Seed: Inserted {Count} countries", seedCountries.Count);
    }
}

// ── Middleware pipeline ─────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HS Codes API v1"));
}

app.UseCors("AllowSPA");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

// ── Map endpoints ───────────────────────────────────────────────
app.MapSearchEndpoints();
app.MapBrowseEndpoints();
app.MapDutyEndpoints();
app.MapNoteEndpoints();
app.MapCodeListEndpoints();
app.MapAdminEndpoints();

app.Run();

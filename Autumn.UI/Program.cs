using System.Net;
using Autumn.Domain.Data;
using Autumn.Domain.Infra;
using Autumn.Domain.Models;
using Autumn.Infrastructure;
using Autumn.Service;
using Autumn.Service.Interface;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;

// Add services to the container
builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("http://34.69.79.114:8003/")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("10.128.0.5"));
    options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
    options.KnownProxies.Add(IPAddress.Parse("162.241.137.245"));
    options.KnownProxies.Add(IPAddress.Parse("34.69.79.114"));
    options.AllowedHosts.Add("http://34.69.79.114:8003");
});

// Database setup
builder.Services.AddDbContext<classificationContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<StoreDatabaseSettings>(
configuration.GetSection(nameof(StoreDatabaseSettings)));

builder.Services.AddSingleton<IStoreDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<StoreDatabaseSettings>>().Value);

builder.Services.AddRepositoryServices();
builder.Services.AddBusinessServices();

builder.Services.AddSingleton<IExRate, ExRate>();
builder.Services.AddSingleton<ITokenizer, Tokenizer>();
builder.Services.AddScoped<IClassification, Classification>();

// AutoMapper configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//// JWT Authentication setup
//var jwtSettings = new JwtSettings();
//configuration.Bind(nameof(jwtSettings), jwtSettings);
//builder.Services.AddSingleton(jwtSettings);

//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = "Cookies";
//    options.DefaultChallengeScheme = "oidc";
//})
//.AddCookie("Cookies")
//.AddOpenIdConnect("oidc", options =>
//{
//    options.SignInScheme = "Cookies";
//    options.Authority = configuration["SiteSettings:SSOURL"];
//    options.RequireHttpsMetadata = false;
//    options.ClientId = configuration["SiteSettings:ClientId"];
//    options.ClientSecret = jwtSettings.Secret;
//    options.ResponseType = "code id_token";
//    options.SaveTokens = true;
//    options.GetClaimsFromUserInfoEndpoint = true;
//    options.Scope.Add("autumnapi");
//});

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
}).AddFluentValidation();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();

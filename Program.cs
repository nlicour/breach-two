using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
    // .EnableTokenAcquisitionToCallDownstreamApi()
    // .AddInMemoryTokenCaches()
    ;


builder.Services.AddAuthorization(options =>
{
    // options.FallbackPolicy = new AuthorizationPolicyBuilder()
    //     .RequireAuthenticatedUser()
    //     .Build();
});

const string connectionString = "";

builder.Services.AddDbContext<SiteContext>(x => x.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", async ([FromServices] SiteContext context) =>
{
    // return new WeatherForecast(DateOnly.MaxValue, 0, "youp"); ;
    return await context.Set<WeatherForecast>().ToArrayAsync();
}).AllowAnonymous();

app.MapGet("auth/weatherforecast", () =>
{
    return new WeatherForecast(DateOnly.MaxValue, 0, "youp"); ;
    // return await context.Set<WeatherForecast>().ToArrayAsync();
}).RequireAuthorization();

app.MapGet("/weatherforecast/{id}", async ([FromServices] SiteContext context, string id) =>
{
    return await context.Set<WeatherForecast>().Where(x => x.Summary == id).ToArrayAsync();
}).AllowAnonymous();

app.MapGet("sqli/ef/weatherforecast/{id}", async ([FromServices] SiteContext context, string id) =>
{
    string x = $"SELECT [w].[Date], [w].[Summary], [w].[TemperatureC] FROM [weatherforecast] AS [w] WHERE [w].[Summary] = '{id}'";
    Console.WriteLine(x);
    return await context.Set<WeatherForecast>().FromSqlRaw($"{x}").ToArrayAsync();
}).AllowAnonymous();



app.MapGet("/ww", async ([FromServices] SiteContext context) =>
{
    // return new WeatherForecast(DateOnly.MaxValue, 0, "youp"); ;
    return await context.Set<WW>().ToArrayAsync();
}).AllowAnonymous();

app.MapGet("sqli/ef/ww/{id}", async ([FromServices] SiteContext context, string id) =>
{
    string x = $"SELECT [w].[collation_name], [w].[name] FROM [ww] AS [w] WHERE [w].[collation_name] = '{id}'";
    Console.WriteLine(x);
    return await context.Set<WW>().FromSqlRaw($"{x}").ToArrayAsync();
}).AllowAnonymous();

app.UseAuthentication();
app.UseAuthorization();


app.Run();

public record WW(string name, string collation_name);

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

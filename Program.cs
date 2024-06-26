using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SiteContext>( x => x.UseSqlServer(""));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", async (SiteContext context) =>
{
    return await context.Set<WeatherForecast>().ToArrayAsync();
});

app.MapGet("/weatherforecast/{id}", async (SiteContext context, string id) =>
{
    return await context.Set<WeatherForecast>().Where(x => x.Summary == id).ToArrayAsync();
});


app.MapGet("/v2/weatherforecast/{id}", async (SiteContext context, string id) =>
{
    return await context.Set<WeatherForecast>().FromSql($"Select * from wf where Summary = {id}").ToArrayAsync();
    // return await context.Database.SqlQuery<WeatherForecast>($"Select * from wf where Summary = {id}").ToArrayAsync();
})
.WithOpenApi();




app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

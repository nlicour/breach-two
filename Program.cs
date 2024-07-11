using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string connectionString = "";

builder.Services.AddDbContext<SiteContext>( x => x.UseSqlServer(connectionString));

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

app.MapGet("sqli/weatherforcast/{id}", (string id) =>{

    using SqlConnection connection = new SqlConnection(connectionString);

    connection.Open();

    string sql = $"SELECT name, collation_name FROM sys.databases where id = {id}";

    using SqlCommand command = new SqlCommand(sql, connection);

    return GetWF(command.ExecuteReader());
});

IEnumerable<WeatherForecast> GetWF(SqlDataReader reader){
    while (reader.Read())
        {
            yield return new WeatherForecast(
                DateOnly.MinValue,
                12,
                reader.GetString(0)
            );
            // Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
        }

}


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

using Microsoft.EntityFrameworkCore;

public class SiteContext: DbContext{
    public SiteContext(){}
    public SiteContext(DbContextOptions<SiteContext> options) : base(options) { }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<WW> WW { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>().ToTable("weatherforecast").HasNoKey();
        modelBuilder.Entity<WW>().ToTable("ww").HasNoKey();
    }
}
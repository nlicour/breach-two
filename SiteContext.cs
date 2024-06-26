using Microsoft.EntityFrameworkCore;

public class SiteContext: DbContext{
    public SiteContext(){}
    public SiteContext(DbContextOptions<SiteContext> options) : base(options) { }
}
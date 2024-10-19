using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using UserServer.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Applier> Appliers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(builder);
    }
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-E15UN3T;Initial Catalog=UserServerDB;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}
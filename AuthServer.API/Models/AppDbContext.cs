using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SharedLibrary.Models;

namespace AuthServer.API.Models;

public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<UserRefleshToken> UserRefleshTokens { get; set; }


	// Configurationlari Model olaraq yaradiriq, hamisi 1 yerde
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
		optionsBuilder.UseSqlServer("Data Source=DESKTOP-E15UN3T;Initial Catalog=AuthServer;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;");

		return new AppDbContext(optionsBuilder.Options);
	}
}

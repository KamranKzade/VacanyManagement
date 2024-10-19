using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using System.Reflection.Emit;

namespace AdminServer.API.Models;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Vacancy> Vacancies { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<Answer> Answers { get; set; }
	public DbSet<Result> Results { get; set; }
	public DbSet<ApplierForAdmin> Applier { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		modelBuilder.Entity<Result>()
		.HasKey(r => new { r.VacancyId, r.ApplierId }); // Composite primary key

		modelBuilder.Entity<Result>()
			.HasOne(r => r.Vacancy)
			.WithMany(v => v.Results)
			.HasForeignKey(r => r.VacancyId);

		modelBuilder.Entity<Result>()
			.HasOne(r => r.Applier)
			.WithMany(a => a.Results)
			.HasForeignKey(r => r.ApplierId);

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
		base.OnModelCreating(modelBuilder);
	}
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseSqlServer("Data Source=DESKTOP-E15UN3T;Initial Catalog=AdminServerDB;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;");

		return new AppDbContext(optionsBuilder.Options);
	}
}
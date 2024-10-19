using AdminServer.API.Models;
using AdminServer.API.Services.Abstracts;
using AdminServer.API.Services.Concretes;
using Microsoft.EntityFrameworkCore;
using OrderServer.API.Repositories.Concrete;
using Serilog;
using SharedLibrary.Repositories.Abstract;
using SharedLibrary.UnitOfWork.Abstract;
using SharedLibrary.UnitOfWork.Concrete;
using System.Reflection;

namespace AdminServer.API.Extensions;

public static class StartUpExtention
{
	public static IServiceCollection AddDbContextWithExtention(this IServiceCollection services)
	{
		services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlServer("Data Source=DESKTOP-E15UN3T;Initial Catalog=AdminServerDB;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;", opt => opt.EnableRetryOnFailure());
		});
		return services;
	}

	public static IServiceCollection AddAutoMapperWithExtention(this IServiceCollection services)
	{
		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		return services;
	}

	public static IServiceCollection AddScopedWithExtention(this IServiceCollection services)
	{
		services.AddScoped<IVacancyService, VacancyService>();
		services.AddScoped<ICategoryService, CategoryService>();
		services.AddScoped<IResultService, ResultService>();
		services.AddScoped<IQuestionService, QuestionService>();
		services.AddScoped<IApplierService, ApplierService>();
		services.AddScoped<IAnswerService, AnswerService>();
		services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
		services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
		return services;
	}

	public static async void AddMigrationWithExtention(this IServiceProvider provider)
	{
		try
		{
			using (var scope = provider.CreateScope())
			{
				var someService = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				await someService.Database.MigrateAsync();
			}

		}
		catch (Exception ex)
		{
			Log.Error($"Error: {ex.Message}");
		}
	}
}

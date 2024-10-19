using Microsoft.EntityFrameworkCore;
using OrderServer.API.Repositories.Concrete;
using Serilog;
using SharedLibrary.Repositories.Abstract;
using SharedLibrary.UnitOfWork.Abstract;
using SharedLibrary.UnitOfWork.Concrete;
using System.Reflection;
using UserServer.API.Config;
using UserServer.API.Services.Abstracts;
using UserServer.Services.Concretes;

namespace UserServer.API.Extentions;

public static class StartUpExtention
{
    public static IServiceCollection AddDbContextExtention(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer("Data Source=DESKTOP-E15UN3T;Initial Catalog=UserServerDB;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;", opt => opt.EnableRetryOnFailure());
        });
        return services;
    }

    public static IServiceCollection AddAutoMapperExtention(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddScopedExtention(this IServiceCollection services)
    {
        services.AddScoped<IApplierService, ApplierService>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
        return services;
    }

    public static IServiceCollection AddSingletonWithExtention(this IServiceCollection services, IConfiguration configuration)
    {
		var uploadConfig = configuration.GetSection("UploadConfig").Get<UploadConfig>();
		services.AddSingleton(uploadConfig);

		var AttachmentConfig = configuration.GetSection("AttachmentConfig").Get<AttachmentConfig>();
		services.AddSingleton(AttachmentConfig);

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

	public static void AddHttpClientExtention(this IServiceCollection services, IConfiguration configuration)
	{
		// AuthService -in methoduna müraciet 
		services.AddHttpClient("AdminServer", client =>
		{
			client.BaseAddress = new Uri(configuration["Microservices:AdminServer"]);
		});
	}
}

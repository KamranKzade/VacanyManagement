﻿using Serilog;
using System.Reflection;
using AuthServer.API.Models;
using SharedLibrary.Extentions;
using FluentValidation.AspNetCore;
using AuthServer.API.Localizations;
using Microsoft.AspNetCore.Identity;
using AuthServer.API.Configurations;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Services.Abstract;
using AuthServer.API.CustomValidations;
using AuthServer.API.Services.Abstract;
using AuthServer.API.Services.Concrete;
using SharedLibrary.UnitOfWork.Concrete;
using SharedLibrary.UnitOfWork.Abstract;
using SharedLibrary.Repositories.Abstract;
using OrderServer.API.Repositories.Concrete;
using SharedLibrary.Models;

namespace AuthServer.API.Extentions;


public static class StartUpExtention
{
	// Identity-ni sisteme tanidiriq
	public static void AddIdentityWithExtention(this IServiceCollection services)
	{
		// Token-a omur vermek
		services.Configure<DataProtectionTokenProviderOptions>(options =>
		{
			options.TokenLifespan = TimeSpan.FromHours(2);
		});

		// security stampa omur vermek
		services.Configure<SecurityStampValidatorOptions>(opt =>
		{
			opt.ValidationInterval = TimeSpan.FromMinutes(30);
		});

		services.AddIdentity<User, IdentityRole>(opt =>
		{
			opt.User.RequireUniqueEmail = true;
			opt.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_";
			opt.Password.RequiredLength = 6; // uzunluq
			opt.Password.RequireNonAlphanumeric = false; // Simvollar olmasada olar
			opt.Password.RequireLowercase = true; // Kicik herfe mutlerolmalidir
			opt.Password.RequireUppercase = false; // botuk olmasada olar
			opt.Password.RequireDigit = false; // reqem olmasada olar

			opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			opt.Lockout.MaxFailedAccessAttempts = 5;
		}).AddUserValidator<UserValidator>()
		.AddErrorDescriber<LocalizationIdentityErrorDescriber>()
		.AddPasswordValidator<PasswordValidator>()
		.AddDefaultTokenProviders() // Deafult token aliriq
		.AddEntityFrameworkStores<AppDbContext>();

	}

	public static void AddScopeWithExtention(this IServiceCollection services)
	{
		// DI lari sisteme tanitmaq
		services.AddScoped<IAuthenticationService, AuthenticationService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
		services.AddScoped(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));
		services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
	}

	public static void AddDbContextWithExtention(this IServiceCollection services, IConfiguration configuration)
	{
		// Connectioni veririk
		services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlServer("Data Source=DESKTOP-E15UN3T;Initial Catalog=AuthServer;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;", opt => opt.EnableRetryOnFailure());
		});
	}

	public static void AddControllersWithExtention(this IServiceCollection services)
	{
		services.AddControllers()
				.AddFluentValidation(opts => // FluentValidationlari sisteme tanidiriq
				{
					opts.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
				});
	}

	public static void OtherAdditionWithExtention(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<List<Client>>(configuration.GetSection("Clients"));
		// Validationlari 1 yere yigib qaytaririq
		services.UseCustomValidationResponse();
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

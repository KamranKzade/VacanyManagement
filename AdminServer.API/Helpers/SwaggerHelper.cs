using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AdminServer.API.Helpers;

public static class SwaggerHelper
{
	public static void ConfigureSwaggerGen(SwaggerGenOptions options)
	{
		options.SwaggerDoc("v1", new OpenApiInfo
		{
			Version = "v1",
			Title = "Admin Server API",
			Description = "All reponses are in generic response model."
		});
		options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			Name = "Authorization",
			Type = SecuritySchemeType.Http,
			Scheme = "Bearer",
			BearerFormat = "JWT",
			In = ParameterLocation.Header,
			Description = "Please enter 'Bearer' [space] and then your valid JWT token in the text input below."
		});
		OpenApiSecurityRequirement openApiSecurityRequirement = new OpenApiSecurityRequirement();
		openApiSecurityRequirement.Add(new OpenApiSecurityScheme()
		{
			Description = "Please enter Bearer Token",
			In = ParameterLocation.Header, // where to find apiKey, probably in a header Name = "Bearer", //header with api key Type = SecuritySchemeType.ApiKey, // this value is always "apiKey"
			Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
		},
		new List<string>());
		options.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				new List<string>() // No specific scopes required
			}
		});
	}
	public static void ConfigureSwagger(SwaggerOptions swaggerOptions)
	{
		swaggerOptions.RouteTemplate = "swagger/" + "{documentName}/swagger.json";
	}
	public static void ConfigureSwaggerUI(SwaggerUIOptions swaggerUIOptions)
	{
		string swaggerJsonBasePath = string.IsNullOrWhiteSpace(swaggerUIOptions.RoutePrefix) ? "." : "..";
		swaggerUIOptions.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "");
		swaggerUIOptions.RoutePrefix = "swagger";
	}
}
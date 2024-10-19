using AdminServer.API.Extensions;
using AdminServer.API.Helpers;
using SharedLibrary.Extentions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddScopedWithExtention();
builder.Services.AddDbContextWithExtention();
builder.Services.AddAutoMapperWithExtention();
builder.Services.AddCustomTokenAuthWithExtentionShared(builder.Configuration);

builder.Services.AddLoggingWithExtentionShared(builder.Configuration);
builder.Services.AddCorsWithExtentionShared();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => SwaggerHelper.ConfigureSwaggerGen(c));

var app = builder.Build();


app.UseSwagger(c => SwaggerHelper.ConfigureSwagger(c));
app.UseSwaggerUI(c => SwaggerHelper.ConfigureSwaggerUI(c));


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseCors("corsapp");
app.UseAuthorization();

app.MapControllers();

app.Run();

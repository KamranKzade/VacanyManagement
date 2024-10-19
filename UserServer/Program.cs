using UserServer.API.Helpers;
using UserServer.API.Extentions;
using SharedLibrary.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContextExtention();
builder.Services.AddAutoMapperExtention();
builder.Services.AddScopedExtention();
builder.Services.AddSingletonWithExtention(builder.Configuration);
builder.Services.AddCustomTokenAuthWithExtentionShared(builder.Configuration);
builder.Services.AddHttpClientExtention(builder.Configuration);
builder.Services.AddLoggingWithExtentionShared(builder.Configuration);
builder.Services.AddCorsWithExtentionShared();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => SwaggerHelper.ConfigureSwaggerGen(c));

var app = builder.Build();

app.Services.AddMigrationWithExtention();

app.UseSwagger(c => SwaggerHelper.ConfigureSwagger(c));
app.UseSwaggerUI(c => SwaggerHelper.ConfigureSwaggerUI(c));


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseCors("corsapp");
app.UseAuthorization();

app.MapControllers();

app.Run();

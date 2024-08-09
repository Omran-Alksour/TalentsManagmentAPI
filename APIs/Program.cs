using Application;
using Application.Abstractions.Caching;
using Infrastructure;
using Infrastructure.Caching;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Presentation;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
builder.Services.Scan(selector => selector.FromAssemblies(Infrastructure.AssemblyReference.Assembly, Persistence.AssemblyReference.Assembly).AddClasses(false).AsImplementedInterfaces().WithScopedLifetime());

builder.Services.AddPersistence(configuration).AddApplication().AddPresentation().AddInfrastructure();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
       builder =>
       {
           _ = builder.WithOrigins("http://localhost:4200", "http://localhost:5173", "http://localhost:3000") 
                  .AllowAnyHeader().AllowAnyMethod();
       });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();

var app = builder.Build();
app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();
//app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


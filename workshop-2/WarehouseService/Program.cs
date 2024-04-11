using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.OpenApi.Models;
using WarehouseService.GrpcServices;
using WarehouseService.Interceptor;
using WarehouseService.Repositories;
using WarehouseService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IWarehouseService, WarehouseService.Services.WarehouseService>();
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.Interceptors.Add<ErrorInterceptor>();
    options.Interceptors.Add<LogInterceptor>();
}).AddJsonTranscoding();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddControllers();
builder.Services.AddScoped<IWarehouseService, WarehouseService.Services.WarehouseService>();
builder.Services.AddSingleton<IWarehouseRepository, WarehouseMemoryRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGrpcSwagger();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("warehouse", new OpenApiInfo());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/warehouse/swagger.json", "warehouse");
});

app.UseHttpsRedirection();

app.UseAuthorization();



//app.MapControllers();
app.MapGrpcService<WarehouseGrpcService>();

app.Run();

public partial class Program{}
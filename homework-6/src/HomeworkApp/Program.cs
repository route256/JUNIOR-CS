using FluentValidation.AspNetCore;
using HomeworkApp.Bll.Extensions;
using HomeworkApp.Dal.Extensions;
using HomeworkApp.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5213, o => o.Protocols = HttpProtocols.Http2);
});

// Add services to the container.
var services = builder.Services;

//add grpc
services.AddGrpc();

//add validation
services.AddFluentValidation(conf =>
{
    conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
    conf.AutomaticValidationEnabled = true;
});

//add inner dependencies
services
    .AddBllServices()
    .AddDalInfrastructure(builder.Configuration)
    .AddDalRepositories();

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["DalOptions:RedisConnectionString"];
});

services.AddGrpcReflection();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TasksService>();
app.MapGrpcReflectionService();

// enroll migrations
app.MigrateUp();

app.Run();
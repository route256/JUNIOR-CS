using System;

using FluentMigrator.Runner;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Ozon.Route256.Kafka.OrderEventConsumer.Presentation;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build();

if (args.Length > 0 && args[0].Equals("migrate", StringComparison.InvariantCultureIgnoreCase))
{
    using var scope = host.Services.CreateScope();
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

    runner.MigrateUp();
}
else
{
    host.Run();
}

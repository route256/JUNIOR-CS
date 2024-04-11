using System;
using FluentMigrator;

using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Postgres.Persistence.Migrations;

[Migration(1, "Initial migration")]
public sealed class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => @"
";
}

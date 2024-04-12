using System;

using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Common;

public abstract class SqlMigration : IMigration
{
    public void GetUpExpressions(IMigrationContext context)
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));

        context.Expressions.Add(new ExecuteSqlStatementExpression { SqlStatement = GetUpSql(context.ServiceProvider) });
    }

    public void GetDownExpressions(IMigrationContext context) => throw new NotSupportedException();

    protected abstract string GetUpSql(IServiceProvider services);

    object IMigration.ApplicationContext => throw new NotSupportedException();
    string IMigration.ConnectionString => throw new NotSupportedException();
}

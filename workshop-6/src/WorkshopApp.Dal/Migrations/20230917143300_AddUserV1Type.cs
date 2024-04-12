using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143300, TransactionBehavior.None)]
public class AddUserV1Type : Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'users_v1') THEN
            CREATE TYPE users_v1 as
            (
                  id         bigint
                , email      text
                , created_at timestamp with time zone
                , blocked_at timestamp with time zone
            );
        END IF;
    END
$$;";

        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
DO $$
    BEGIN
        DROP TYPE IF EXISTS users_v1;
    END
$$;";

        Execute.Sql(sql);
    }
}
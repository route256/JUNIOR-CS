using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143500, TransactionBehavior.None)]
public class AddTaskLogV1Type : Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'task_logs_v1') THEN
            CREATE TYPE task_logs_v1 as
            (
                  id                  bigint
                , task_id             bigint
                , parent_task_id      bigint
                , number              text
                , title               text
                , description         text
                , status              integer
                , assigned_to_user_id bigint
                , user_id             bigint
                , at                  timestamp with time zone
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
        DROP TYPE IF EXISTS task_logs_v1;
    END
$$;";

        Execute.Sql(sql);
    }
}
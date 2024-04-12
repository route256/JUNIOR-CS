using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143400, TransactionBehavior.None)]
public class AddTaskV1Type : Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'tasks_v1') THEN
            CREATE TYPE tasks_v1 as
            (
                  id                  bigint
                , parent_task_id      bigint
                , number              text
                , title               text
                , description         text
                , status              integer
                , created_at          timestamp with time zone
                , created_by_user_id  bigint
                , assigned_to_user_id bigint
                , completed_at        timestamp with time zone
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
        DROP TYPE IF EXISTS tasks_v1;
    END
$$;";

        Execute.Sql(sql);
    }
}
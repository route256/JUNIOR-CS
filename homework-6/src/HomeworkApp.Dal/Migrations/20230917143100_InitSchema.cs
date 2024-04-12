using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143100, TransactionBehavior.None)]
public class InitSchema : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsInt64().PrimaryKey("users_pk").Identity()
            .WithColumn("email").AsString().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("blocked_at").AsDateTimeOffset().Nullable();
        
        Create.Table("task_statuses")
            .WithColumn("id").AsInt64().PrimaryKey("task_statuses_pk")
            .WithColumn("alias").AsString().NotNullable()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("description").AsString().Nullable();
        
        Create.Table("tasks")
            .WithColumn("id").AsInt64().PrimaryKey("tasks_pk").Identity()
            .WithColumn("parent_task_id").AsInt64().Nullable()
            .WithColumn("number").AsString().NotNullable()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("description").AsString().Nullable()
            .WithColumn("status").AsInt32().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("created_by_user_id").AsInt64().NotNullable()
            .WithColumn("assigned_to_user_id").AsInt64().Nullable()
            .WithColumn("completed_at").AsDateTimeOffset().Nullable();

        Create.Table("task_logs")
            .WithColumn("id").AsInt64().PrimaryKey("task_logs_pk").Identity()
            .WithColumn("task_id").AsInt64()
            .WithColumn("parent_task_id").AsInt64().Nullable()
            .WithColumn("number").AsString().NotNullable()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("description").AsString().Nullable()
            .WithColumn("status").AsInt32().NotNullable()
            .WithColumn("assigned_to_user_id").AsInt64().Nullable()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("at").AsDateTimeOffset().NotNullable();
        
        Create.Table("task_comments")
            .WithColumn("id").AsInt64().PrimaryKey("task_comments_pk").Identity()
            .WithColumn("task_id").AsInt64().NotNullable()
            .WithColumn("author_user_id").AsInt64().NotNullable()
            .WithColumn("message").AsString().NotNullable()
            .WithColumn("at").AsDateTimeOffset().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("users");
        Delete.Table("task_statuses");
        Delete.Table("tasks");
        Delete.Table("task_logs");
        Delete.Table("task_comments");
    }
}
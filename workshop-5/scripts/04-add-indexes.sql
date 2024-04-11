create index if not exists ix_task_comments_task_id
    on task_comments(task_id);
create index if not exists ix_task_comments_author_user_id
    on task_comments(author_user_id);
create index if not exists ix_tasks_parent_task_id
    on tasks (parent_task_id);
create index if not exists ix_tasks_status
    on tasks (status);
create index if not exists ix_tasks_created_by_user_id
    on tasks (created_by_user_id);
create index if not exists ix_tasks_assigned_to_user_id
    on tasks (assigned_to_user_id);
create unique index if not exists iux_task_statuses_alias
    on task_statuses (alias);
create unique index if not exists iux_users_email
    on users (email);
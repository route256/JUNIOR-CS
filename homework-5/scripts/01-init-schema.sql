/* USERS */
create table if not exists users
(
      id bigserial primary key
    , email text not null
    , created_at timestamptz not null
    , blocked_at timestamptz
);

/* TASK_STATUSES */
create table if not exists task_statuses 
(
    id int primary key 
  , alias text not null
  , name text not null
  , description text
);

/* TASKS */
create table if not exists tasks
(
    id bigserial primary key 
  , parent_task_id bigint
  , number text not null
  , title text not null
  , description text
  , status int not null 
  , created_by_user_id bigint not null 
  , assigned_to_user_id bigint
  , created_at timestamptz not null
  , completed_at timestamptz

  , foreign key (parent_task_id) references tasks(id)
  , foreign key (status) references task_statuses(id)
  , foreign key (created_by_user_id) references users(id)
  , foreign key (assigned_to_user_id) references users(id)
);

/* TASK_COMMENTS */
create table if not exists task_comments
(
    id bigserial primary key 
  , task_id bigint not null
  , author_user_id bigint not null
  , message text not null
  , at timestamptz not null

  , foreign key (task_id) references tasks(id)
  , foreign key (author_user_id) references users(id)
);

/* TASK_LOGS */
create table if not exists task_logs
(
      id bigserial primary key
    , task_id bigint not null
    , parent_task_id bigint
    , number text not null
    , title text not null
    , description text
    , status int not null
    , created_by_user_id bigint not null
    , assigned_to_user_id bigint
    , user_id bigint not null
    , at timestamptz not null
);
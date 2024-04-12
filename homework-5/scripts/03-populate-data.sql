/* TRUNCATE */
delete from task_logs;
delete from task_comments;
delete from tasks;
delete from users;

alter sequence users_id_seq restart with 1;
alter sequence tasks_id_seq restart with 1;


/* POPULATE USERS */
insert into users (email, created_at, blocked_at)
    select substr(md5(num::text), 0, 8) || '@random.ru'
         , now() - random()*interval '180 days'
         , case when num % floor(random() * 13 + 1)::int = 0
                then now() + case when num % 2 = 0
                                  then 1
                                  else -1
                             end
                           * random()*interval '7 days'
           end
      from generate_series(0, 999) num;

/* POPULATE TASKS */
insert into tasks (number, title, description, status, created_by_user_id, created_at)
    select 'N/A'
         , 'N/A'
         , 'N/A'
         , (random() * 4)::int + 1
         , (random() * 998)::int + 1
         , now()
      from generate_series(0, 9999) num;

/* FILL TASKS CREATION DATE */
update tasks t 
   set created_at = u.created_at + random()*interval '100 days'
  from users u 
 where u.id = t.created_by_user_id;

/* FILL TASKS NUMBERS */
with tasks_in_day
  as (select to_char(t.created_at, 'YYYYMMDD') as number_day_part
           , array_agg(t.id order by t.created_at) as ids
        from tasks t
       group by to_char(t.created_at, 'YYYYMMDD'))
update tasks t
   set number = d.number_day_part || '-' || array_position(d.ids, t.id)
     , title  = 'Очень важное задание №' || number::text
     , description = number::text || ': здесь должно быть описание, что нужно сделать'
  from tasks_in_day d
 where t.id = any(d.ids);

/* ASSIGN TASKS TO USERS */
update tasks t
   set assigned_to_user_id = (select u.id
                                from users u
                               where u.created_at <= t.created_at
                                 and (    u.blocked_at > t.created_at
                                      or u.blocked_at is null
                                      or t.status in (4, 5))
                               order by (random() * 99)::int
                               limit 1)
 where status > 2;

/* POPULATE LOGS */
insert into task_logs (task_id, parent_task_id, number, title, description, status, created_by_user_id, assigned_to_user_id, user_id, at)
    select t.id
         , t.parent_task_id
         , t.number
         , t.title
         , t.description
         , ts.id
         , t.created_by_user_id
         , case when ts.id in (1, 2)
                then null
                else t.assigned_to_user_id
           end
         , case when ts.id in (1, 2, 5)
                then t.created_by_user_id
                else coalesce(t.assigned_to_user_id, t.created_by_user_id)
           end
         , t.created_at + (ts.id - 1) * (random()*120 + 1) * interval '1 minutes'
      from tasks t
      join task_statuses ts on ts.id <= t.status
     order by t.id
            , ts.id;

/* FILL COMPLETED AT */
update tasks t
   set completed_at = (select tl.at
                         from task_logs tl
                        where tl.task_id = t.id
                          and tl.status = 4
                        order by tl.id desc
                        limit 1)
where t.status = 4;

/* POPULATE COMMENTS */
with conversations 
  as (select t.id as task_id
           , t.created_by_user_id
           , t.assigned_to_user_id
           , num as index
           , (random()* 10 + 1)::int as messages_count
           , md5(num::text || '@' || t.created_by_user_id::text || '#' || coalesce(assigned_to_user_id, 0)::text) as message
           , t.created_at
        from tasks t
           , generate_series(1, 10) num
       where t.status != 1
       order by t.id)
insert into task_comments (task_id, author_user_id, message, at) 
    select c.task_id
         , case when c.index % 2 = 0 and c.assigned_to_user_id is not null
                then c.assigned_to_user_id
                else c.created_by_user_id
           end
         , c.message
         , c.created_at + random()*interval '7 days'
      from conversations c 
     where c.index <= c.messages_count
       and c.task_id % (random()*6 + 1)::int = 0;

/* CONVERT SOME TASKS TO SUBTASKS */
with sub_tasks
    as (select t.id
          from tasks t
         where t.parent_task_id is null
         order by random()
         limit 110)
   , parent_tasks
    as (select t.id
          from tasks t
         where t.parent_task_id is null
           and not exists (select 1
                             from sub_tasks ss
                            where ss.id = t.id))
   , tasks_subtasks
    as (select p.id as task_id
             , (select array_agg(s.id)
                  from sub_tasks s
                 where s.id between p.id - 20 and p.id + 20) as sub_tasks
          from parent_tasks p)
update tasks t
   set parent_task_id = ts.task_id
  from tasks_subtasks ts
 where t.id = any(ts.sub_tasks);


with sub_tasks
    as (select t.id
          from tasks t
         where t.parent_task_id is null
         order by random()
         limit 80)
   , parent_tasks
    as (select t.id
          from tasks t
         where t.parent_task_id is not null
           and not exists (select 1
                             from sub_tasks ss
                            where ss.id = t.id))
   , tasks_subtasks
    as (select p.id as task_id
             , (select array_agg(s.id)
                  from sub_tasks s
                 where s.id between p.id - 20 and p.id + 20) as sub_tasks
          from parent_tasks p)
update tasks t
   set parent_task_id = ts.task_id
  from tasks_subtasks ts
 where t.id = any(ts.sub_tasks);




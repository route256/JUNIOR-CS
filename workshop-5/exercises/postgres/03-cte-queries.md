## Вежливое приветственное сообщение по заданию (cte)
Дополнить сообщения по заданию приветственным и прощальным сообщением с учетом времени суток, когда задание было создано или выполнено.
В результате вывести:
- сообщение
- email автора сообщения

<details>
  <summary>Решение</summary>

```sql
with task_info
  as (select id                              as task_id
           , date_part('hour', created_at)   as created_at_hour
           , date_part('hour', completed_at) as completed_at_hour
        from tasks
       where id = :task_id)
select case when ti.created_at_hour between  6 and 10 then 'Доброе утро!'
            when ti.created_at_hour between 11 and 17 then 'Добрый день!'
            when ti.created_at_hour between 18 and 20 then 'Добрый вечер!'
            else 'Доброй ночи!'
       end             as message
     , 'n/a'           as author_email
  from task_info ti
union all
select tc.message
     ,  u.email
  from task_comments tc
  join users u on tc.author_user_id = u.id
 where tc.task_id = :task_id
union all
select case when ti.completed_at_hour between  6 and 17 then 'Хорошего дня!'
            when ti.completed_at_hour between 18 and 20 then 'Хорошего вечера!'
            else 'Спокойной ночи!'
       end             as message
     , 'n/a'           as author_email
  from task_info ti
```
</details>

## Проверка завершенности подзаданий (recursive cte)
Перед тем, как завершить задание, нужно проверить, что все дочерние задания завершены.
Необходима выборка всех дочерних заданий конкретного задания со статусами.
В результате вывести:
- ИД задания
- ИД родительского задания
- уровень задания в иерархии
- статус задания

<details>
  <summary>Решение</summary>

```sql
with recursive tasks_tree
  as (select t.id
           , t.parent_task_id
           , t.status
           , 1 as level
           , '/' || t.id::text as path
        from tasks t
       where t.id = :task_id
      union all
      select t.id
           , t.parent_task_id
           , t.status
           , tt.level + 1 as level
           , tt.path || '/' || t.id::text as path
        from tasks t
        join tasks_tree tt on t.parent_task_id = tt.id)
select *
from tasks_tree
```
</details>

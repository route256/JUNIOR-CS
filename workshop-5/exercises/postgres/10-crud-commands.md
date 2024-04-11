## Создать копию задания (insert)
Создать копию существующего задания.
В результате вывести:
- ИД созданного задания

<details>
  <summary>Решение</summary>

```sql
insert into tasks (parent_task_id, number, title, description, status, created_by_user_id, assigned_to_user_id, created_at, completed_at) 
    select parent_task_id
         , number
         , title
         , description
         , status
         , created_by_user_id
         , assigned_to_user_id
         , now()
         , completed_at
      from tasks t
     where t.id = :task_id
   returning id;
```
</details>

## Добавление или изменение статуса (upsert)
Добавить новый стаус задания OnHold, если он уже есть то просто изменить его название и описание.
параметры статуса:
- ИД = 8
- псевдоним - OnHold
- название - Приостановлено
- описание - Выполнение приостановлено по требованию заказчика

<details>
  <summary>Решение</summary>

```sql
insert into task_statuses (id, alias, name, description)
  values (8, 'OnHold', 'Приостановлено', 'Выполнение приостановлено по требованию заказчика')
on conflict (id)
    do update set alias = 'OnHold'
                , name = 'Приостановлено'
                , description = 'Выполнение приостановлено по требованию заказчика';
```
</details>

## Блокировка неактивных пользователей (update)
Блокировтаь пользователя через 60 дней после последнй активности, если пользователь был неактивен в течение 60 дней (не было изменений по заданиям)
В результате вывести:
- ИД блокируемого пользователя
- метку времени, после которой пользователь будет заблокирован

<details>
  <summary>Решение</summary>

```sql
with users_to_block
         as (select tl.user_id as user_id
                  , max(tl.at) as last_activity
               from task_logs tl
              group by tl.user_id
             having max(tl.at) < now() - interval '60 days')
   update users u
      set blocked_at = ub.last_activity + interval '120 day'
     from users_to_block ub
    where u.id = ub.user_id
returning id
        , blocked_at;
```
</details>

## Удаление логов по отменненным заданиям (delete)
Удалять логи по отмененным заданиям.
В результате вывести:
- ИД заданий, из которых были удалены логи

<details>
  <summary>Решение</summary>

```sql
   delete from task_logs tl
    where exists (select 1
                    from tasks t
                   where t.status = 5 /* Canceled */
                     and t.id = tl.task_id)
returning task_id;
```
</details>


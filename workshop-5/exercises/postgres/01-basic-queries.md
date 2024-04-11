## Задания со статусами (inner join)
Получить 10 самых новых заданий, отсортированных от самых новых к самым старым.
В результате вывести:
- номер задания
- заголовок задания
- название статуса задания
- email автора задания

<details>
  <summary>Решение</summary>

```sql
   select  t.number as task_number
        ,  t.title  as task_title
        , ts.name   as task_status
        , cu.email  as author_email
     from tasks t
     join task_statuses ts on ts.id = t.status
     join users cu on cu.id = t.created_by_user_id
    order by t.created_at desc
    limit 10;
```
</details>

## Задания по исполнителям (outer join)
Получить 10 самых новых заданий с исполнителями.
В результате вывести:
- номер задания
- заголовок задания
- email исполнителя задания

<details>
  <summary>Решение</summary>

```sql
   select  t.number as task_number
        ,  t.title  as task_title
        , au.email  as author_email
     from tasks t
left join users au on au.id = t.assigned_to_user_id
    order by t.created_at desc
    limit 10;
```
</details>

## Дата назначения заданий на исполнителя (sub queries)
Получить метку времени, когда последние 10 заданий были назначены на текущего исполнителя.
В результате вывести:
- номер задания
- заголовок задания
- дату и время назначения задания на текущего исполнителя

<details>
  <summary>Решение</summary>

```sql
select  t.number   as task_number
     ,  t.title    as task_title
     ,  (select tl.at
           from task_logs tl
          where tl.task_id = t.id
            and tl.status  = 3 /* InProgress */
            and tl.assigned_to_user_id = t.assigned_to_user_id
          order by tl.id desc
          limit 1) as task_assigned_at
  from tasks t
  limit 10
```
</details>

## Авторы заданий (distinct)
Получить идентификаторы авторов заданий, которые создавали задания за указанный период времени.
В результате вывести:
- ИД автора

<details>
  <summary>Решение</summary>

```sql
select distinct
       created_by_user_id
  from tasks
 where created_at between '20230601' and '20230602'
```
</details>

## Приветственное сообщение по заданию (union / union all)
Дополнить сообщения по заданию приветственным и прощальным сообщением.
В результате вывести:
- сообщение
- email автора сообщения

<details>
  <summary>Решение</summary>

```sql
  select 'Здравствуйте!' as message
       , 'n/a'           as author_email
  union all
  select tc.message
       ,  u.email
  from task_comments tc
           join users u on tc.author_user_id = u.id
  where tc.task_id = :task_id
  union all
  select 'До свидания!' as message
       , 'n/a'          as author_email
```
</details>

## Задания, которых нет в выборке (except)
Дан массив из 3х заданий, нужно найте те, которые не были созданы в указанный период времени.
В результате вывести:
- ИД здания

<details>
  <summary>Решение</summary>

```sql
select id
  from (values (114)
             , (115)
             , (116)) as t(id)
except
select t.id
  from tasks t
 where t.created_at between '20230601' and '20230602'
```
</details>

## Выборка заданий с пагинацией (limit / offset)
Получить список заданий за указанный период времени с пагинацией, отсортированный по дате создания задания.
В результате вывести:
- номер задания
- заголовок задания
- дата создания

<details>
  <summary>Решение</summary>

```sql

select t.number
     , t.title
     , t.created_at
  from tasks t
 where t.created_at between '20230601' and '20230602'
 order by t.id
 limit :limit
offset :offset
```
</details>


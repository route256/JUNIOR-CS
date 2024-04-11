## Количество заданий на исполнителях (count)
Узнать, сколько активных заданий (не в статусах Done, Canceled) сейчас на каждом исполнителе.
Отсортировать по количеству заданий - первым должен быть исполнитель с наибольшим количеством заданий.
В результате вывести:
- email исполнителя
- количество заданий
- количество заданий на заблокированных пользователях

<details>
  <summary>Решение</summary>

```sql
select u.email as assignee_email
     , count(1) as tasks_count
     , count(1) filter (where blocked_at is not null) as on_blocked_assignees
  from tasks t
  join users u on u.id = t.assigned_to_user_id
 where status not in (4, 5) /* Done, Canceled */
 group by u.email
 order by 2 desc;
```
</details>

## Среднее время выполнения задания (avg)
Определить среднее время выполнения задания в часах каждого автора.
В результате вывести:
- email автора задания
- среднее количество часов

<details>
  <summary>Решение</summary>

```sql
select u.email as author_email
     , avg(date_part('hour', t.completed_at - t.created_at)) as avg_work_hours
  from tasks t
  join users u on u.id = t.created_by_user_id
 where t.status = 4 /* Done */
 group by u.email;
```
</details>


## Исполнители с большим количеством заданий (having)
Найти исполнителей, на который более 10 незавершенных заданий.
Отсортировать по количеству заданий - первым должен быть исполнитель с наибольшим количеством заданий.
В результате вывести:
- email исполнителя
- количество заданий

<details>
  <summary>Решение</summary>

```sql
select u.email as assignee_email
     , count(1) as tasks_count
  from tasks t
  join users u on u.id = t.assigned_to_user_id
 where status not in (4, 5) /* Done, Canceled */
 group by u.email
having count(1) > :tasks_threshold
 order by 2 desc;
```
</details>



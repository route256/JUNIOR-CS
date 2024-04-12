## Анализ плана выполнения (explain)
Найти пользователей, которые писали сообщения по заявке после того, как она перешла в стаусе InProgress.
Проанализировтаь план выполнения.
В результате вывести:
- email автора сообщений

<details>
  <summary>Решение</summary>

```sql
explain (analyze, buffers)
select distinct
     u.email
  from task_logs tl
  join task_comments tc on tc.task_id = tl.task_id
                       and tc.at >= tl.at
  join users u on u.id = tc.author_user_id
 where tl.status = 2 /* InProgress */;
```
</details>



## Нумерция сообщений по порядку (row_number)
Пронумеровть сообщения в задании по порядку.
В результате вывести:
- ИД задания
- номер сообщения по порядку
- сообщение
- метка времени отправки сообщения

<details>
  <summary>Решение</summary>

```sql
select c.task_id
     , row_number() over (partition by c.task_id order by c.at)
     , c.message
     , c.at
  from task_comments c
 order by 1, 2
```
</details>


## Карта перехода заданиq по статусам (lag/lead)
В коде нет модели перехода заданий по статусам, нужно понять с какого статуса можно перейти в какой.
В результате вывести:
- статус с котоорго переходим
- статус в который переходим

<details>
  <summary>Решение</summary>

```sql
select distinct
       tl.status
     , lead(tl.status) over (partition by tl.task_id order by tl.id)
  from task_logs tl
 where tl.at between '20230101' and '20231231'
   and exists (select 1
                 from tasks t
                where t.id = tl.task_id
                  and t.status in (4, 5))
 order by 1;
```
</details>

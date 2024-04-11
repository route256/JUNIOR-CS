## Количество суббот в 2023 году 
Написать выборку, возвращающую количество суббот в каждом месяце 2023 года
В результате вывести:
- номер месяца
- количество суббот

<details>
  <summary>Решение</summary>

```sql

with dates
  as (select '20230101'::timestamp + num * interval '1 day' as date
        from generate_series(0, 365) num)
select date_part('month', date) as month
     , sum(case when date_part('dow', date) = 6
                then 1
                else 0
           end) as saturdays_count
  from dates
 group by date_part('month', date)
 order by 1;
```
</details>

## Преобразование в массив и из массива
Получить масссив идентификаторов задание за указанный промежуток врмеени

<details>
  <summary>Решение</summary>

```sql
select array_agg(t.id order by t.status)
  from tasks t
 where created_at between '20230601 00:00:00' and '20230601 03:00:00';

select *
  from task_logs tl
 where tl.task_id = any(array[2365,1126,7051,738])
```
</details>

## Преобразование строки в json и обратно
Получить задания как объекты  json

<details>
  <summary>Решение</summary>

```sql

select row_to_json(t) as json
     , row_to_json(t)->'id' as id
from tasks t
where created_at between '20230601 00:00:00' and '20230601 03:00:00'

```
</details>
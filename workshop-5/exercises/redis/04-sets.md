## Наборы

Наполнение набора значений
```redis
sadd actual_tasks_set 1 2 3 4 5

sadd old_tasks_set 2 4 6 7
```

Получаем элементы наборов
```redis
smembers actual_tasks_set

smembers old_tasks_set
```

Операции с наборами
```redis
sdiff actual_tasks_set old_tasks_set

sinter actual_tasks_set old_tasks_set

sunion actual_tasks_set old_tasks_set
```
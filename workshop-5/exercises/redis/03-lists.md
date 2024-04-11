## Списки

Наполнение списка значений
```redis
lpush active_tasks 1 2 3

rpush active_tasks 100 200 300
```

Вставка значения в середину списка
```redis
linsert active_tasks before 2 18

linsert active_tasks after 100 130
```

Получение списка
```redis
lrange active_tasks 0 -1
```

Извлечение значения из списка
```redis
lpop active_tasks

rpop active_tasks

lrem active_tasks -1 100
```
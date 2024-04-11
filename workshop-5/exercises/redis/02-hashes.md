## Хэшсеты

Создание ключа с хэшсетом
```redis
hset tasks:10 id 10 status 3 title "test123" parent_task_id null
hset tasks:11 id 11 status 2 title "Новое задание" parent_task_id null assignee 100
```

Получение значений по ключу
```redis
hget tasks:10 title

hmget tasks:10 id status

hgetall tasks:10
```
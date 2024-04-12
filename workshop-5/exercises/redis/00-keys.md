## Работа с ключами
Удаление ключа
```redis
del users:10
```

Указание TLL для ключа
```redis
expire users:09 45
```

Проверка TTL по ключу
```redis
ttl users:09
```

Получение типа ключа
```redis
type users:10
```

Выборка ключей
```redis
keys users:*
```

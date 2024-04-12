## Сортированные наборы

Наполнение набора значений
```redis
zadd top_assignee 10 test@random.ru

zadd top_assignee 34 test2@random.ru

zadd top_assignee 8 test4@random.ru
```

Получаем элементы наборов с сортировкой по весам
```redis
zrange top_assignee 0 -1 WITHSCORES

zrevrange top_assignee 0 -1 WITHSCORES
```

Получение веса элемента
```redis
zscore top_assignee test4@random.ru
```

Изменение веса элемента
```redis
zincrby top_assignee 22 test4@random.ru
```

Получение позиции элемента в наборе
```redis
zrank top_assignee test4@random.ru
zrevrank top_assignee test4@random.ru
```
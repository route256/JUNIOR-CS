## Миграции
### Мигрируем схему данных
```shell
psql -h localhost -p 15432 -U postgres -d workshop-5 -a -f ../scripts/01-init-schema.sql
```

### Сидим данные
```shell
psql -h localhost -p 15432 -U postgres -d workshop-5 -a -f ../scripts/02-seed-data.sql
```

### Наполняем схему мок данными
```shell
psql -h localhost -p 15432 -U postgres -d workshop-5 -a -f ../scripts/03-populate-data.sql
```
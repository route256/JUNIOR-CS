# PostgreSql и Redis  на практике

## Перед тем как начать
- Как подготовить окружение [см. тут](./docs/01-prepare-environment.md)
- Как накатить миграции на базу данных [см. тут](./docs/02-data-migrations.md)
- Удачный пример, как форматировать SQL, чтобы его всегда можно было прочитать [см. тут](./docs/04-sql-guidelines.md)
- **САМОЕ ВАЖНОЕ** - полное описание базы данных, схему и описание полей можно найти [тут](./docs/03-db-description.md)

## Упражнения по PostgreSql
- [Базовые запросы выборки данных](./exercises/postgres/01-basic-queries.md)
  - соединения таблиц, подзапросы
  - объединение и исключение результатов
  - уникальность данных и постраничный вывод
- [Запросы с агрегационными функциями](./exercises/postgres/02-aggregation-queries.md)
  - агрегационые функции
  - исключение результатов после агрегации
- [Общие табличные выражения](./exercises/postgres/03-cte-queries.md)
  - использование cte
  - рекурсивные запросы
- [Оконные функции (промежуточная агрегация)](./exercises/postgres/04-window-functions-queries.md)
  - функции оконного вычисления (промежуточной агрегации)
- [Базовые команды изменения данных](./exercises/postgres/10-crud-commands.md)
  - вставка, удаление, изменение
  - операции изменения, если данные уже есть (upsert)
- [План выполнения запроса](./exercises/postgres/20-execution-plan.md)
  - пример для анализа плана запроса
- [Прочее](./exercises/postgres/99-miscellaneous.md)
  - работа с не-sql типами данных (json, array)
  - генераторы

## Упражнения по Redis
- [Операции с ключами](./exercises/redis/00-keys.md)
- [Строковые данные](./exercises/redis/01-strings.md)
- [Хэши](./exercises/redis/02-hashes.md)
- [Списки](./exercises/redis/03-lists.md)
- [Наборы](./exercises/redis/04-sets.md)
- [Сортированные наборы](./exercises/redis/05-sorted-sets.md)
- [Подписка на события](./exercises/redis/06-pub-sub.md)
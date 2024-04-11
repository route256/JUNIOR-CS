# Неделя 6: домашнее задание

## Перед тем как начать
- Как подготовить окружение [см. тут](./docs/01-prepare-environment.md)
- **САМОЕ ВАЖНОЕ** - полное описание базы данных, схему и описание поле можно найти [тут](./docs/03-db-description.md)

## Основные требования
- каждый сервис уникальный с точки зрения кодовой базы, не нужно заниматься рефакторингом, вводить новые уровни абстракций или применять дополнительные паттерны проектирования (друими словами, подстраивать код под себя), нужно написать методы в том же стиле, что и существующие
- все методы в репозиториях должны быть покрыты интеграционными тестами
- интеграционные тесты должны проверять корректность запроса к БД и маппинга полученных результатов
- если для задачи требуется хардкод каких-то статусов и т.п. нужно использовать константы в C# коде и пробрасывать их параметрами в SQL
- все даты всегда `DateTimeOffset` или `timestamp with time zone` и всегда UTC

## Задание 1: Метод проверки перед завершением задания
Нужен один параметризуемый метод в репозитории, который бы прошелся рекурсивно по всем дочерним заданиям и вернул  только те подзадачи, которые находятся в указанных статусах. В ответе нужно вернуть ИД подзадачи, его заголовок, статус и путь от родительского задания к этой дочерней задаче. 

<details>
  <summary>Где это может быть использовано</summary>

Задачи могут иметь дочерние задачи (подзадачи). Для того, чтобы завершить основное задание (перевести в статус Done), нужно, чтобы все дочерние задания были завершены (Done) или отменены (Canceled). Для того, чтобы отменить основное задание (перевести в Canceled) нужно, чтобы все дочерние задания были отменены (Canceled).
</details>

**Контракт:**
```csharp
public interface ITaskRepository
{
    ...
    Task<SubTaskModel[]> GetSubTasksInStatus(long parentTaskId, TaskStatus[] statuses, CancellationToken token);
}
...
public record SubTaskModel
{
    public required long TaskId { get; init; }
    public required string Title { get; init; }
    public required TaskStatus Status { get; init; }
    public required long[] ParentTaskIds { get; init; }
}
```

## Задание 2: Операции над сообщениями по заданию
Добавить новые поля в таблицу task_comments через миграции:
- поле `modified_at timestamp with time zone null`-  будет заполняться при изменении сообщения
- поле `deleted_at timestamp with time zone null` - будет заполняться при удалении сообщения

Реализовать методы pg-репозитория:

- Метод получения сообщений по заданию, отсортированных от самого позднего к самому раннему. Предусмотреть фильтр для получения в том числе удаленных заданий (по умолчанию удаленные задания не возвращаются)
- Метод добавления нового сообщения
- Метод изменения сообщения (указывает modified_at как текущую дату UTC)
- Метод отметки сообщения как удаленного (указывает deleted_at как текущую дату UTC)

**Контракты:**
```csharp
public interface ITaskCommentRepository
{
    Task<long> Add(TaskCommentEntityV1 model, CancellationToken token);
    Task Update(TaskCommentEntityV1 model, CancellationToken token);
    Task SetDeleted(long taskCommentId, CancellationToken token);
    Task<TaskCommentEntityV1[]> Get(TaskCommentGetModel model, CancellationToken token);
}
...
public record TaskCommentGetModel
{
    public required long TaskId { get; init; }
    public required bool IncludeDeleted { get; init; }
}
```

В HomeworkApp.Bll реализовать метод в TaskService для получения сообщений по заданию, который бы использовал Redis-cache. В кэш помещать последние 5 сообщений (самые новые) и возвращать их из кэша. Время жизни кэша - 5сек.

```csharp
public interface ITaskService
{
    ...
    Task<TaskMessage[]> GetComments(long taskId, CancellationToken token);
}
...
public record TaskMessage
{
    public required long TaskId { get; init; }
    public required string Comment { get; init; }
    public required bool IsDeleted { get; init; }
    public required DateTimeOffset At { get; init; }
}
```


## Задание 3 (за алмазик): RateLimiter
На основе Redis реализовать рейт-лимитер с ограничением в 100 запросов в минуту. Можно допустить, что IP-адрес пользователя можно получить из заголовка запроса X-R256-USER-IP.
Встроить в Middleware или Interceptor и, если превышен лимит, падать с ошибкой.
Покрыть рейт-лимитер юнит-тестами, которые бы провели основные кейсы:
- успешное выполнение запроса
- превышение лимита на выполнение

**ВАЖНО!** Цель этого задания сделать "на коленке" свою реализацию рейт-лимитера с использованием Redis и поработать с Redis из C#, поэтому использовать готовые пакеты (вроде redis-rate-limiting) не допускается.
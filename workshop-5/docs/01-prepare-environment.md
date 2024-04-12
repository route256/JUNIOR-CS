## Подготовка окружения
### Предварительные требования
1. Установлен docker desktop или colima
2. Демон docker desktop или colima запущены
3. Установлен соответствующий менеджер пакетов:
   - MacOs - homebrew
   - Windows - chocolatey
   - Ubuntu - apt-get

### Установка psql
<details>
  <summary>Установка на MacOS</summary>

```shell
brew install libpq
brew link --force libpq
```
</details>

<details>
  <summary>Установка на Ubuntu</summary>

```shell
sudo apt-get update
sudo apt-get install postgresql-client
```
</details>

<details>
  <summary>Установка на Windows</summary>

```shell
choco install postgresql
```
</details>

Полный гайд [см. тут](https://www.timescale.com/blog/how-to-install-psql-on-mac-ubuntu-debian-windows/)

### Установка Redis
<details>
  <summary>Установка на MacOS</summary>

```shell
brew install redis
```
</details>

<details>
  <summary>Установка на Ubuntu</summary>

```shell
sudo apt-get install redis
```
</details>

<details>
  <summary>Установка на Windows</summary>

см. интрукцию [тут](https://redis.io/docs/getting-started/installation/install-redis-on-windows)
</details>

### Запускаем контейнер
```shell
docker-compose up -d
```
### Проверка подключения к PostgreSql
В ответе должен показать **localhost:15432 - accepting connections**
```shell
pg_isready -h localhost -p 15432 -U postgres -d workshop-5
```
### Проверка подключения к Redis
В ответе должен показать **PONG**
```shell
redis-cli -h localhost -p 16379 PING
```

# 🐱 CatStore
RESTful веб-сервис интернет магазина по продаже виртуальных котят.

# 🔥 Возможности
* Наполнение каталога магазина котятами.
* Регистрация новых пользователей и авторизация на основе JWT токенов.
* Управление покупками посредством корзины товаров.
* Формирование заказов пользователей.

# 🧭 Дорожная карта
* Автоматизация сборки при помощи [Nuke](https://nuke.build).
* Полноценный frontend на [React](https://react.dev/blog/2023/03/16/introducing-react-dev).
* Покрытие кода тестами.
* Добавление CI/CD.
* Интеграция с [ЮКасса](https://yookassa.ru).

# 📑 Документация
* [Спецификация по WebAPI](https://github.com/Neitralov/CatStore/blob/master/docs/CatStore.WebAPI.yaml)
* [Схема архитектуры проекта](https://github.com/Neitralov/CatStore/blob/master/docs/CatStore-arch.png)
* [ER-модель](https://github.com/Neitralov/CatStore/blob/master/docs/CatStore%20ER-model.png)
* [Ограничения по стилю кода](https://github.com/Neitralov/CatStore/blob/master/docs/Code%20style.md)

# 🛠️ Сборка
1. Установите [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) версии 8.0 или новее.
2. Убедитесь, что имеете [Docker](https://docs.docker.com/get-docker/) или [Podman](https://podman.io).
3. Скачайте исходники и запустите соответствующий скрипт `build-and-run_docker.sh` или `build-and-run_podman.sh`.

После приложение будет доступно в браузере по адресу `localhost:8000`.

Документация по WebAPI доступна по адресу: `localhost:8000/swagger`.

# 🚀 Запуск
Для запуска подготовлен Docker образ приложения:

```
docker run \
-d \
-p 8000:8080 \
-v database:/app/Database:Z \
-e AppSettings:Token="My secret key. 128 bit at least." \
-e ConnectionStrings:DefaultConnection="Data Source=Database/Database.db" \
--name catstore \
neitralov/catstore:latest
```
После приложение будет доступно в браузере по адресу `localhost:8000`.

P.S. установите переменную окружения `ASPNETCORE_ENVIRONMENT=Development`, чтобы получить доступ к SwaggerUI.

# 🧰 Стек технологий
* Веб-фреймворк: [ASP.NET Core 8](https://dotnet.microsoft.com/en-us/apps/aspnet)
* ORM: [EF Core 7](https://learn.microsoft.com/ru-ru/ef/core/)
* СУБД: [SQLite](https://www.sqlite.org/about.html)

Дополнительные пакеты:

* [ErrorOr](https://github.com/amantinband/error-or)
* [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

# 📃 Лицензия:
Программа распространяется под лицензией [MIT](https://github.com/Neitralov/CatStore/blob/master/LICENSE).

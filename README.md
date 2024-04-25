# 🐱 CatStore
CatStore — учебный проект, реализующий электронный магазин по продаже виртуальных котиков.

# 🌆 Скриншоты
![image](https://github.com/Neitralov/CatStore/assets/109409226/9f788b0d-6517-48ac-92bf-713c445fc98b)
<details>
  <summary>Смотреть остальные</summary>

  ![image](https://github.com/Neitralov/CatStore/assets/109409226/557b6c35-0c3a-42c3-85cd-8d8bd2bfda70)
  ![image](https://github.com/Neitralov/CatStore/assets/109409226/e610f19b-bdf7-435c-95a9-a3068362bed4)
  ![image](https://github.com/Neitralov/CatStore/assets/109409226/8cc15f42-4614-40af-9d3b-6e7b081a9cb5)
  ![image](https://github.com/Neitralov/CatStore/assets/109409226/758388f4-6e07-42ca-9732-816f13d50ca3)
</details>
<details>
  <summary>Скриншоты с телефона</summary>

  ![image](https://github.com/Neitralov/CatStore/assets/109409226/a44f56e6-3acc-454f-af84-07417cf20df5)
  ![image](https://github.com/Neitralov/CatStore/assets/109409226/69082618-0176-4719-9662-aefd59557a9c)
  ![image](https://github.com/Neitralov/CatStore/assets/109409226/3e0729d7-f8b6-4e2f-bade-2f96572d3690)
  ![image](https://github.com/Neitralov/CatStore/assets/109409226/72b07fe9-db28-4299-a180-9a2a73e1a0bd)
  ![image](https://github.com/Neitralov/CatStore/assets/109409226/18dc5795-8d1e-4fc4-8e9d-e7955d12a471)
  ![image](https://github.com/Neitralov/CatStore/assets/109409226/299da835-f5b6-4e85-916f-1d0e4e2fbdd7)
</details>

# 🔥 Особенности
* Адаптивный интерфейс для устройств любого размера.
* Просмотр товаров в каталоге с возможностью фильтрации и сортировки.
* Поиск товара по названию.
* Просмотр описания товара на отдельной странице.
* Формирование заказа посредством корзины товаров.
* Локальная корзина товаров для сбора заказа без регистрации до самого момента оформления.
* Оформление заказов и их последующая оплата (интеграция с [ЮКасса](https://yookassa.ru)).
* Просмотр истории заказов.
* Наполнение каталога товарами для администратора с помощью редактора котиков.
* Система скидок на товары.
* Регистрация и авторизация пользователей.
* Возможность смены пароля после регистрации.

# 📑 Документация
* [Макет сайта](https://www.figma.com/file/UFocNVkF2bDlpVsSXGmOxW/CatStore)
* [Схема архитектуры проекта](https://github.com/Neitralov/CatStore/blob/master/docs/CatStore-arch.png)
* [ER-модель](https://github.com/Neitralov/CatStore/blob/master/docs/CatStore%20ER-model.png)
* [Ограничения по стилю кода](https://github.com/Neitralov/CatStore/blob/master/docs/Code%20style.md)

# 🛠️ Сборка
1. Установите [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) версии 8.0 или новее.
2. Убедитесь, что имеете [Podman](https://podman.io).
3. Скачайте исходники и запустите скрипт `run.sh`.

Для работы оформления платежей необходимо создать [тестовый магазин ЮKassa](https://yookassa.ru/developers/payment-acceptance/getting-started/quick-start) и добавить ID и Токен в `run.sh` 

```
# Иначе заказ будет оформлен без создания платежа
-e Yookassa:Id="123456" \
-e Yookassa:Token="Your yookassa token" \
```

После приложение будет доступно в браузере по адресу http://localhost:8080.

Данные для входа (почта/пароль): `admin@gmail.com` `admin`

# 🧰 Стек технологий
Frontend:

* Веб-сервер: [Nginx](http://nginx.org/en/)
* Веб-фреймворк: [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
* CSS-фреймворк: [Tailwindcss](https://tailwindcss.com/)

Backend:

* Веб-фреймворк: [ASP.NET Core 8](https://dotnet.microsoft.com/en-us/apps/aspnet)
* ORM: [EF Core 8](https://learn.microsoft.com/ru-ru/ef/core/)
* СУБД: [PostgreSQL](https://www.postgresql.org/)

Дополнительные пакеты:

* [ErrorOr](https://github.com/amantinband/error-or)
* [Blazored.LocalStorage](https://github.com/Blazored/LocalStorage)
* [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [Swashbuckle.AspNetCore.Filters](https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters)
* [Serilog.AspNetCore](https://github.com/serilog/serilog-aspnetcore)
* [Mapster](https://github.com/MapsterMapper/Mapster)

# 📃 Лицензия
Программа распространяется под лицензией [MIT](https://github.com/Neitralov/CatStore/blob/master/LICENSE).

За исключением шрифта приложения Roboto [Apache License 2.0](https://github.com/Neitralov/CatStore/blob/fcfbb7a9aea2b73032b2cc2d38ccda1313bae3c1/src/Client/wwwroot/css/LICENSE.txt).

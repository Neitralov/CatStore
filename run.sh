#!/bin/bash

# Переходим в папку веб-API, компилируем и собираем docker image
cd src/Server/WebAPI
dotnet build -c Debug
podman build . -t webapitest

# Переходим в папку с клиентом, компилируем и собираем docker image
cd ../../Client
bun tailwindcss -i wwwroot/css/tailwind.css -o wwwroot/css/app.css
dotnet publish -c Release
podman build . -t webclienttest

# Создаем Pod
podman pod create \
--name catstore \
-p 8080:80 \
-p 8081:8080 \
-p 5432:5432 \
--replace

# Запускаем контейнер с БД
podman run \
-d \
--pod catstore \
-v catstore-volume:/var/lib/postgresql/data:Z \
-e POSTGRES_DB=catstore \
-e POSTGRES_USER=postgres \
-e POSTGRES_PASSWORD=1234 \
--name catstore-postgres \
--replace \
postgres:16.2

sleep 3

# Запускаем контейнер с backend-ом
podman run \
-d \
--pod catstore \
-e ASPNETCORE_ENVIRONMENT=Development \
-e FrontendUrl="http://localhost:8080" \
-e AppSettings:Token="My favorite really secret key. 512 bit at least. (64 characters)." \
-e ConnectionStrings:DefaultConnection="Host=catstore-postgres;Port=5432;Database=catstore;Username=postgres;Password=1234" \
-e Yookassa:Id="123456" \
-e Yookassa:Token="Your yookassa token" \
--name catstore-webapi \
--replace \
webapitest

# Запускаем контейнер с frontend-ом
podman run \
-d \
--pod catstore \
--name catstore-client \
--replace \
webclienttest
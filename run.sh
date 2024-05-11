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
-p 27017:27017 \
--replace

# Запускаем контейнер с БД
podman run \
-d \
--pod catstore \
-v catstore-volume:/data/db:Z \
--name catstore-mongodb \
--replace \
mongo:7.0.9

sleep 2

# Запускаем контейнер с backend-ом
podman run \
-d \
--pod catstore \
-e ASPNETCORE_ENVIRONMENT=Development \
-e FrontendUrl="http://localhost:8080" \
-e AppSettings:Token="My favorite really secret key. 512 bit at least. (64 characters)." \
-e ConnectionStrings:DefaultConnection="mongodb://catstore-mongodb:27017" \
-e DatabaseName="catstore" \
-e Yookassa:Id="374735" \
-e Yookassa:Token="test_dUbdl7ejvqx5_S6_rZyYxv-ta0fp88EpuLzJQj5xBPw" \
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
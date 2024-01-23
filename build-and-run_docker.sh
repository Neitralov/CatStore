#!/bin/bash

cd src/WebAPI

echo "Компилируется проект..."
dotnet build -c Debug

echo "Остановка предыдущего контейнера..."
docker stop test

echo "Собирается Docker образ программы..."
docker build . -t webapitest

echo "Запускается контейнер с программой..."
docker run \
-d \
--rm \
-p 8000:8080 \
-v database:/app/Database:Z \
-e ASPNETCORE_ENVIRONMENT=Development \
-e AppSettings:Token="My favorite really secret key. 512 bit at least. (64 characters)." \
-e ConnectionStrings:DefaultConnection="Data Source=Database/Database.db" \
--name test \
webapitest
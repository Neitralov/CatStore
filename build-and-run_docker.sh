#!/bin/bash

cd src/WebAPI

echo "Компилируется проект..."
dotnet build -c Release

echo "Остановка предыдущего контейнера..."
docker stop test

echo "Собирается Docker образ программы..."
docker build . -t webapitest

echo "Запускается контейнер с программой..."
docker run \
-d \
--rm \
-p 8000:80 \
-v database:/app/Database:Z \
-e ASPNETCORE_ENVIRONMENT=Development \
--name test \
webapitest

echo "Готово"
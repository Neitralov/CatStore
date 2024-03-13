#!/bin/bash

cd src/Server/WebAPI

echo "Компилируется backend..."
dotnet build -c Debug

podman stop test

echo "Собирается Docker образ backend-a..."
podman build . -t webapitest

echo "Запускается контейнер с backend-ом..."
podman run \
-d \
--rm \
-p 8000:8080 \
-v ./CatStore:/app/Database:Z \
-e ASPNETCORE_ENVIRONMENT=Development \
-e AppSettings:Token="My favorite really secret key. 512 bit at least. (64 characters)." \
-e ConnectionStrings:DefaultConnection="Data Source=Database/Database.db" \
--name test \
webapitest

cd ../../Client

echo "Запускается frontend..."
bun tailwindcss -i wwwroot/css/tailwind.css -o wwwroot/css/app.css
dotnet run
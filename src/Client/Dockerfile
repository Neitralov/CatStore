FROM nginx:alpine3.18
WORKDIR /app
COPY ./bin/Release/net8.0/publish/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
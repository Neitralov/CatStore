FROM mcr.microsoft.com/dotnet/aspnet:7.0 
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY ./bin/Debug/net7.0 .
ENTRYPOINT ["dotnet", "WebAPI.dll"]
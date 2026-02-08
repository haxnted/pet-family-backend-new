# Используем официальный .NET SDK образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Build arguments для GitHub Packages
ARG GITHUB_USERNAME
ARG GITHUB_TOKEN

# Устанавливаем переменные окружения для NuGet
ENV GITHUB_USERNAME=${GITHUB_USERNAME}
ENV GITHUB_TOKEN=${GITHUB_TOKEN}

# Копируем проекты для восстановления зависимостей
COPY ["Solution/Notification/Hosts/Notification.Api/Notification.Api.csproj", "Solution/Notification/Hosts/Notification.Api/"]
COPY ["Solution/Notification/Notification.Application/Notification.Application.csproj", "Solution/Notification/Notification.Application/"]
COPY ["Solution/Notification/Notification.Infrastructure/Notification.Infrastructure.csproj", "Solution/Notification/Notification.Infrastructure/"]
COPY ["Solution/Notification/Notification.Core/Notification.Core.csproj", "Solution/Notification/Notification.Core/"]
COPY ["Solution/Notification/Notification.Contracts/Notification.Contracts.csproj", "Solution/Notification/Notification.Contracts/"]

# Копируем nuget.config
COPY ["nuget.config", "./"]

# Восстанавливаем зависимости
RUN dotnet restore "Solution/Notification/Hosts/Notification.Api/Notification.Api.csproj"

# Копируем весь исходный код
COPY . .

# Собираем проект
WORKDIR "/src/Solution/Notification/Hosts/Notification.Api"
RUN dotnet build "Notification.Api.csproj" -c Release -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "Notification.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Копируем опубликованное приложение
COPY --from=publish /app/publish .

# Устанавливаем переменные окружения
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Notification.Api.dll"]

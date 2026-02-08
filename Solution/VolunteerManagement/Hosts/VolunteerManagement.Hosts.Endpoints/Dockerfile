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
COPY ["Solution/VolunteerManagement/Hosts/VolunteerManagement.Endpoints/VolunteerManagement.Endpoints.csproj", "Solution/VolunteerManagement/Hosts/VolunteerManagement.Endpoints/"]
COPY ["Solution/VolunteerManagement/Application/VolunteerManagement.Handlers/VolunteerManagement.Handlers.csproj", "Solution/VolunteerManagement/Application/VolunteerManagement.Handlers/"]
COPY ["Solution/VolunteerManagement/Application/VolunteerManagement.Services/VolunteerManagement.Services.csproj", "Solution/VolunteerManagement/Application/VolunteerManagement.Services/"]
COPY ["Solution/VolunteerManagement/VolunteerManagement.Infrastructure/VolunteerManagement.Infrastructure.csproj", "Solution/VolunteerManagement/VolunteerManagement.Infrastructure/"]
COPY ["Solution/VolunteerManagement/VolunteerManagement.Domain/VolunteerManagement.Domain.csproj", "Solution/VolunteerManagement/VolunteerManagement.Domain/"]
COPY ["Solution/VolunteerManagement/VolunteerManagement.Contracts/VolunteerManagement.Contracts.csproj", "Solution/VolunteerManagement/VolunteerManagement.Contracts/"]

# Копируем nuget.config
COPY ["nuget.config", "./"]

# Восстанавливаем зависимости
RUN dotnet restore "Solution/VolunteerManagement/Hosts/VolunteerManagement.Endpoints/VolunteerManagement.Endpoints.csproj"

# Копируем весь исходный код
COPY . .

# Собираем проект
WORKDIR "/src/Solution/VolunteerManagement/Hosts/VolunteerManagement.Endpoints"
RUN dotnet build "VolunteerManagement.Endpoints.csproj" -c Release -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "VolunteerManagement.Endpoints.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Копируем опубликованное приложение
COPY --from=publish /app/publish .

# Устанавливаем переменные окружения
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "VolunteerManagement.Endpoints.dll"]

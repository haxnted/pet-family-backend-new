# Dockerfile для FileStorage.Endpoints
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем файл Directory.Build.props
COPY ["FileStorage/Directory.Build.props", "FileStorage/"]

# Копируем все .csproj файлы
COPY ["FileStorage/Hosts/FileStorage.Endpoints/FileStorage.Endpoints.csproj", "FileStorage/Hosts/FileStorage.Endpoints/"]
COPY ["FileStorage/FileStorage.Application/FileStorage.Application.csproj", "FileStorage/FileStorage.Application/"]
COPY ["FileStorage/FileStorage.Infrastructure/FileStorage.Infrastructure.csproj", "FileStorage/FileStorage.Infrastructure/"]
COPY ["FileStorage/FileStorage.Contracts/FileStorage.Contracts.csproj", "FileStorage/FileStorage.Contracts/"]

# Restore зависимостей
RUN dotnet restore "FileStorage/Hosts/FileStorage.Endpoints/FileStorage.Endpoints.csproj"

# Копируем весь исходный код
COPY FileStorage/ FileStorage/

# Build проекта
WORKDIR "/src/FileStorage/Hosts/FileStorage.Endpoints"
RUN dotnet build "FileStorage.Endpoints.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FileStorage.Endpoints.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileStorage.Endpoints.dll"]

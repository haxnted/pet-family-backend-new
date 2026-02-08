# Dockerfile для FileStorage.Consumers
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем файл Directory.Build.props
COPY ["FileStorage/Directory.Build.props", "FileStorage/"]

# Копируем все .csproj файлы
COPY ["FileStorage/Hosts/FileStorage.Consumers/FileStorage.Consumers.csproj", "FileStorage/Hosts/FileStorage.Consumers/"]
COPY ["FileStorage/FileStorage.Infrastructure/FileStorage.Infrastructure.csproj", "FileStorage/FileStorage.Infrastructure/"]
COPY ["FileStorage/FileStorage.Contracts/FileStorage.Contracts.csproj", "FileStorage/FileStorage.Contracts/"]

# Restore зависимостей
RUN dotnet restore "FileStorage/Hosts/FileStorage.Consumers/FileStorage.Consumers.csproj"

# Копируем весь исходный код
COPY FileStorage/ FileStorage/

# Build проекта
WORKDIR "/src/FileStorage/Hosts/FileStorage.Consumers"
RUN dotnet build "FileStorage.Consumers.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FileStorage.Consumers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileStorage.Consumers.dll"]

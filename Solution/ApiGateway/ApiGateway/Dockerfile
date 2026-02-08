# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Build arguments for GitHub Packages
ARG GITHUB_USERNAME
ARG GITHUB_TOKEN

# Set environment variables for NuGet
ENV GITHUB_USERNAME=${GITHUB_USERNAME}
ENV GITHUB_TOKEN=${GITHUB_TOKEN}

# Copy NuGet config
COPY ["nuget.config", "./"]

# Copy project file and restore dependencies
COPY ["Solution/ApiGateway/ApiGateway/ApiGateway.csproj", "Solution/ApiGateway/ApiGateway/"]

RUN dotnet restore "Solution/ApiGateway/ApiGateway/ApiGateway.csproj"

# Copy all source code
COPY . .

# Build the project
WORKDIR "/src/Solution/ApiGateway/ApiGateway"
RUN dotnet build "ApiGateway.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy published application
COPY --from=publish /app/publish .

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Docker

ENTRYPOINT ["dotnet", "ApiGateway.dll"]

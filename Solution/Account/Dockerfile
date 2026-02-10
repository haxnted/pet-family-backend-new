# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Build arguments for GitHub authentication
ARG GITHUB_TOKEN
ARG GITHUB_USERNAME

# Copy Directory.Packages.props, Directory.Build.props and nuget.config
COPY ["Directory.Packages.props", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["nuget.Docker.config", "nuget.config"]

# Configure GitHub Packages authentication
RUN dotnet nuget update source github \
    --username ${GITHUB_USERNAME} \
    --password ${GITHUB_TOKEN} \
    --store-password-in-clear-text

# Copy Shared projects
COPY ["Shared/PetFamily.SharedKernel.Domain/PetFamily.SharedKernel.Domain.csproj", "Shared/PetFamily.SharedKernel.Domain/"]
COPY ["Shared/PetFamily.SharedKernel.Application/PetFamily.SharedKernel.Application.csproj", "Shared/PetFamily.SharedKernel.Application/"]
COPY ["Shared/PetFamily.SharedKernel.Contracts/PetFamily.SharedKernel.Contracts.csproj", "Shared/PetFamily.SharedKernel.Contracts/"]
COPY ["Shared/PetFamily.SharedKernel.Infrastructure/PetFamily.SharedKernel.Infrastructure.csproj", "Shared/PetFamily.SharedKernel.Infrastructure/"]
COPY ["Shared/PetFamily.SharedKernel.WebApi/PetFamily.SharedKernel.WebApi.csproj", "Shared/PetFamily.SharedKernel.WebApi/"]

# Copy FileStorage.Contracts (dependency of Account.Hosts.DI)
COPY ["Solution/FileStorage/FileStorage.Contracts/FileStorage.Contracts.csproj", "Solution/FileStorage/FileStorage.Contracts/"]

# Copy Account projects
COPY ["Solution/Account/Account.Domain/Account.Domain.csproj", "Solution/Account/Account.Domain/"]
COPY ["Solution/Account/Account.Infrastructure/Account.Infrastructure.csproj", "Solution/Account/Account.Infrastructure/"]
COPY ["Solution/Account/Application/Account.Services/Account.Services.csproj", "Solution/Account/Application/Account.Services/"]
COPY ["Solution/Account/Application/Account.Handlers/Account.Handlers.csproj", "Solution/Account/Application/Account.Handlers/"]
COPY ["Solution/Account/Hosts/Account.Hosts.Consumers/Account.Hosts.Consumers.csproj", "Solution/Account/Hosts/Account.Hosts.Consumers/"]
COPY ["Solution/Account/Hosts/Account.Hosts.DI/Account.Hosts.DI.csproj", "Solution/Account/Hosts/Account.Hosts.DI/"]
COPY ["Solution/Account/Hosts/Account.Hosts.Endpoints/Account.Hosts.Endpoints.csproj", "Solution/Account/Hosts/Account.Hosts.Endpoints/"]

# Restore dependencies
RUN dotnet restore "Solution/Account/Hosts/Account.Hosts.Endpoints/Account.Hosts.Endpoints.csproj"

# Copy everything else
COPY ["Shared/", "Shared/"]
COPY ["Solution/Account/", "Solution/Account/"]
COPY ["Solution/FileStorage/FileStorage.Contracts/", "Solution/FileStorage/FileStorage.Contracts/"]

# Build
WORKDIR "/src/Solution/Account/Hosts/Account.Hosts.Endpoints"
RUN dotnet build "Account.Hosts.Endpoints.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Account.Hosts.Endpoints.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Account.Hosts.Endpoints.dll"]

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

# Copy Auth projects
COPY ["Solution/Auth/Auth.Core/Auth.Core.csproj", "Solution/Auth/Auth.Core/"]
COPY ["Solution/Auth/Auth.Contracts/Auth.Contracts.csproj", "Solution/Auth/Auth.Contracts/"]
COPY ["Solution/Auth/Auth.Application/Auth.Application.csproj", "Solution/Auth/Auth.Application/"]
COPY ["Solution/Auth/Auth.Infrastructure/Auth.Infrastructure.csproj", "Solution/Auth/Auth.Infrastructure/"]
COPY ["Solution/Auth/Hosts/Auth.Endpoints/Auth.Endpoints.csproj", "Solution/Auth/Hosts/Auth.Endpoints/"]

# Restore dependencies
RUN dotnet restore "Solution/Auth/Hosts/Auth.Endpoints/Auth.Endpoints.csproj"

# Copy everything else
COPY ["Shared/", "Shared/"]
COPY ["Solution/Auth/", "Solution/Auth/"]

# Build
WORKDIR "/src/Solution/Auth/Hosts/Auth.Endpoints"
RUN dotnet build "Auth.Endpoints.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Auth.Endpoints.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.Endpoints.dll"]

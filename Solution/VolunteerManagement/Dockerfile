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

# Copy VolunteerManagement projects
COPY ["Solution/VolunteerManagement/VolunteerManagement.Domain/VolunteerManagement.Domain.csproj", "Solution/VolunteerManagement/VolunteerManagement.Domain/"]
COPY ["Solution/VolunteerManagement/VolunteerManagement.Infrastructure/VolunteerManagement.Infrastructure.csproj", "Solution/VolunteerManagement/VolunteerManagement.Infrastructure/"]
COPY ["Solution/VolunteerManagement/Application/VolunteerManagement.Services/VolunteerManagement.Services.csproj", "Solution/VolunteerManagement/Application/VolunteerManagement.Services/"]
COPY ["Solution/VolunteerManagement/Application/VolunteerManagement.Handlers/VolunteerManagement.Handlers.csproj", "Solution/VolunteerManagement/Application/VolunteerManagement.Handlers/"]
COPY ["Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Consumers/VolunteerManagement.Hosts.Consumers.csproj", "Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Consumers/"]
COPY ["Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.DI/VolunteerManagement.Hosts.DI.csproj", "Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.DI/"]
COPY ["Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Endpoints/VolunteerManagement.Hosts.Endpoints.csproj", "Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Endpoints/"]

# Restore dependencies
RUN dotnet restore "Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Endpoints/VolunteerManagement.Hosts.Endpoints.csproj"

# Copy everything else
COPY ["Shared/", "Shared/"]
COPY ["Solution/VolunteerManagement/", "Solution/VolunteerManagement/"]

# Build
WORKDIR "/src/Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Endpoints"
RUN dotnet build "VolunteerManagement.Hosts.Endpoints.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "VolunteerManagement.Hosts.Endpoints.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VolunteerManagement.Hosts.Endpoints.dll"]

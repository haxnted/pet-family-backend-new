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

# Copy Conversation projects
COPY ["Solution/Conversation/Conversation.Domain/Conversation.Domain.csproj", "Solution/Conversation/Conversation.Domain/"]
COPY ["Solution/Conversation/Conversation.Infrastructure/Conversation.Infrastructure.csproj", "Solution/Conversation/Conversation.Infrastructure/"]
COPY ["Solution/Conversation/Application/Conversation.Services/Conversation.Services.csproj", "Solution/Conversation/Application/Conversation.Services/"]
COPY ["Solution/Conversation/Application/Conversation.Handlers/Conversation.Handlers.csproj", "Solution/Conversation/Application/Conversation.Handlers/"]
COPY ["Solution/Conversation/Hosts/Conversation.Hosts.DI/Conversation.Hosts.DI.csproj", "Solution/Conversation/Hosts/Conversation.Hosts.DI/"]
COPY ["Solution/Conversation/Hosts/Conversation.Hosts.Endpoints/Conversation.Hosts.Endpoints.csproj", "Solution/Conversation/Hosts/Conversation.Hosts.Endpoints/"]

# Restore dependencies
RUN dotnet restore "Solution/Conversation/Hosts/Conversation.Hosts.Endpoints/Conversation.Hosts.Endpoints.csproj"

# Copy everything else
COPY ["Shared/", "Shared/"]
COPY ["Solution/Conversation/", "Solution/Conversation/"]

# Build
WORKDIR "/src/Solution/Conversation/Hosts/Conversation.Hosts.Endpoints"
RUN dotnet build "Conversation.Hosts.Endpoints.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Conversation.Hosts.Endpoints.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conversation.Hosts.Endpoints.dll"]

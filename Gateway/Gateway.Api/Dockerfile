# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["shared/Directory.Packages.props", "shared/"]
COPY ["ChatApp.Aspire/Directory.Packages.props", "ChatApp.Aspire/"]
COPY ["Directory.Build.props", "."]
COPY ["Gateway/Gateway.Api/Gateway.Api.csproj", "Gateway/Gateway.Api/"]
COPY ["shared/DTOs/DTOs.csproj", "shared/DTOs/"]
COPY ["shared/Enums/Enums.csproj", "shared/Enums/"]
COPY ["ChatApp.Aspire/ChatApp.Aspire.ServiceDefaults/ChatApp.Aspire.ServiceDefaults.csproj", "ChatApp.Aspire/ChatApp.Aspire.ServiceDefaults/"]
COPY ["Gateway/Gateway.Application/Gateway.Application.csproj", "Gateway/Gateway.Application/"]
COPY ["Gateway/Gateway.Domain/Gateway.Domain.csproj", "Gateway/Gateway.Domain/"]
COPY ["Gateway/Gateway.Infrastructure/Gateway.Infrastructure.csproj", "Gateway/Gateway.Infrastructure/"]
RUN dotnet restore "./Gateway/Gateway.Api/Gateway.Api.csproj"
COPY . .
WORKDIR "/src/Gateway/Gateway.Api"
RUN dotnet build "./Gateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.Api.dll"]
# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservice/BackgroundService/P2P.Process.Worker/P2P.Process.Worker.csproj", "Microservice/BackgroundService/P2P.Process.Worker/"]
RUN dotnet restore "./Microservice/BackgroundService/P2P.Process.Worker/P2P.Process.Worker.csproj"
COPY . .
WORKDIR "/src/Microservice/BackgroundService/P2P.Process.Worker"
RUN dotnet build "./P2P.Process.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./P2P.Process.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "P2P.Process.Worker.dll"]
﻿FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProductAPI/ProductAPI.csproj", "ProductAPI/"]
COPY ["ProductAPI.Infrastructure/ProductAPI.Infrastructure.csproj", "ProductAPI.Infrastructure/"]
COPY ["ProductAPI.ApplicationCore/ProductAPI.ApplicationCore.csproj", "ProductAPI.ApplicationCore/"]
RUN dotnet restore "ProductAPI/ProductAPI.csproj"
COPY . .
WORKDIR "/src/ProductAPI"
RUN dotnet build "ProductAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV angularURL='http://localhost:4200'
ENTRYPOINT ["dotnet", "ProductAPI.dll"]

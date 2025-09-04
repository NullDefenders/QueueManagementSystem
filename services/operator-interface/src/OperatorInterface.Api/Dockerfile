FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OperatorInterface.Api/OperatorInterface.Api.csproj", "src/OperatorInterface.Api/"]
COPY ["src/OperatorInterface.Core/OperatorInterface.Core.csproj", "src/OperatorInterface.Core/"]
COPY ["src/OperatorInterface.Infrastructure/OperatorInterface.Infrastructure.csproj", "src/OperatorInterface.Infrastructure/"]
COPY ["src/OperatorInterface.Queries/OperatorInterface.Queries.csproj", "src/OperatorInterface.Queries/"]
RUN dotnet restore "src/OperatorInterface.Api/OperatorInterface.Api.csproj"
COPY . .
WORKDIR "/src/src/OperatorInterface.Api"
RUN dotnet build "./OperatorInterface.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OperatorInterface.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:8080
ENTRYPOINT ["dotnet", "OperatorInterface.Api.dll"]

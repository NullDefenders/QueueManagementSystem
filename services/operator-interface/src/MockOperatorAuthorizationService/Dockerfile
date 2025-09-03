FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MockOperatorAuthorizationService/MockOperatorAuthorizationService.csproj", "src/MockOperatorAuthorizationService/"]
RUN dotnet restore "src/MockOperatorAuthorizationService/MockOperatorAuthorizationService.csproj"
COPY . .
WORKDIR "/src/src/MockOperatorAuthorizationService"
RUN dotnet build "./MockOperatorAuthorizationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MockOperatorAuthorizationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockOperatorAuthorizationService.dll"]

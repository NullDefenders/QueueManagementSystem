FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OperatorInterface.BlazorApp/OperatorInterface.BlazorApp.csproj", "src/OperatorInterface.BlazorApp/"]
RUN dotnet restore "src/OperatorInterface.BlazorApp/OperatorInterface.BlazorApp.csproj"
COPY . .
WORKDIR "/src/src/OperatorInterface.BlazorApp"
RUN dotnet build "./OperatorInterface.BlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OperatorInterface.BlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OperatorInterface.BlazorApp.dll"]

# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8087

ENV ASPNETCORE_URLS=http://0.0.0.0:8087
ENV ASPNETCORE_HTTP_PORTS=8087


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Domain/Domain.Entities/Domain.Entities.csproj", "Domain/Domain.Entities/"]
COPY ["Infrastructure/Infrastructure.EntityFrameWork/Infrastructure.EntityFrameWork.csproj", "Infrastructure/Infrastructure.EntityFrameWork/"]
COPY ["PreRegistrationService/PreRegistrationService.csproj", "PreRegistrationService/"]


RUN dotnet restore "PreRegistrationService/PreRegistrationService.csproj"

COPY . .

WORKDIR "/src/PreRegistrationService"

RUN dotnet build "./PreRegistrationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PreRegistrationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PreRegistrationService.dll"]
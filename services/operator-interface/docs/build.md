
# OpenApi

openapi.yaml - OperatorInterface.Api/Adapters/Http/openapi.yaml

Вызывать из папки OperatorInterface.Api/Adapters/Http/Contract
```
cd OperatorInterface.Api/Adapters/Http/Contract/
openapi-generator generate -i ../openapi.yml -g aspnetcore -o . --package-name OpenApi --additional-properties classModifier=abstract --additional-properties operationResultTask=true
```
# БД
```
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
```

# Миграции
```
dotnet ef migrations add Init --startup-project ./OperatorInterface.Api --project ./OperatorInterface.Infrastructure --output-dir ./Adapters/Postgres/Migrations
dotnet ef database update --startup-project ./OperatorInterface.Api --connection "Server=localhost;Port=5432;User Id=username;Password=secret;Database=operator_interface;"
```

# Запросы к БД
```

```

# Сертификаты
```
dotnet dev-certs https --clean
dotnet dev-certs https -ep $env:USERPROFILE/.aspnet/https/aspnetapp.pfx -p P@ssw0rd1
dotnet dev-certs https --trust
dir ~/.aspnet/https


```
using MockOperatorAuthorizationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Настройка gRPC
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Настройка pipeline
app.MapGrpcService<MockOperatorAuthorizationService.Services.MockOperatorAuthorizationService>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGet("/", () => "gRPC Mock Operator Authorization Service is running. Use gRPC client to connect.");

// Логирование информации о сервисе
app.Logger.LogInformation("Mock gRPC service started");
app.Logger.LogInformation("Available test operators:");
app.Logger.LogInformation("- ivanov / password123 / WP005");
app.Logger.LogInformation("- petrov / password456 / WP007");
app.Logger.LogInformation("- sidorov / password789 / WP001 (inactive)");

app.Run();
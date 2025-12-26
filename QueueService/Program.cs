using QueueService.Domain;
using QueueService.Factory;
using QueueService.Services;
using QueueService.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Add(new RemoteSettingsConfigurationSource(
        builder.Configuration,
        builder.Logging.Services.BuildServiceProvider()
               .GetRequiredService<ILoggerFactory>()));

// Регистрируем процессоры сообщений
builder.Services.AddSingleton<TalonMessageProcessor>();
builder.Services.AddSingleton<WindowMessageProcessor>();
builder.Services.AddSingleton<MessageProcessorFactory>();

builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddSingleton<RedisService>();

builder.Services.AddHostedService<RabbitMQBackgroundService>();


var app = builder.Build();


app.Run();

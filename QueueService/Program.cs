using QueueService.Domain;
using QueueService.Factory;
using QueueService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Регистрируем процессоры сообщений
builder.Services.AddSingleton<TalonMessageProcessor>();
builder.Services.AddSingleton<WindowMessageProcessor>();
builder.Services.AddSingleton<MessageProcessorFactory>();

builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddSingleton<RedisService>();

builder.Services.AddHostedService<RabbitMQBackgroundService>();


var app = builder.Build();


app.Run();

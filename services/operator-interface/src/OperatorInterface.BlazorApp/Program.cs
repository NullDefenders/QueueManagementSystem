using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using OperatorInterface.BlazorApp;
using OperatorInterface.BlazorApp.Components;
using OperatorInterface.BlazorApp.Services;
using OperatorInterface.Core.Application.UseCases.Commands.AssignClient;
using OperatorInterface.Core.Application.UseCases.Commands.AuthorizeOperator;
using OperatorInterface.Core.Application.UseCases.Commands.CloseOperatorSession;
using OperatorInterface.Core.Application.UseCases.Commands.CompleteClientSession;
using OperatorInterface.Core.Application.UseCases.Commands.MarkClientAsNotCame;
using OperatorInterface.Core.Application.UseCases.Commands.OpenOperatorSession;
using OperatorInterface.Core.Application.UseCases.Commands.RequestClient;
using OperatorInterface.Core.Application.UseCases.Commands.StartClientSession;
using OperatorInterface.Core.Application.UseCases.Commands.StartWork;
using OperatorInterface.Core.Domain.SharedKernel;
using OperatorInterface.Core.Ports;
using OperatorInterface.Infrastructure.Adapters.Grpc.AuthorizationService;
using OperatorInterface.Infrastructure.Adapters.Nats.RequestClientAssignment;
using OperatorInterface.Infrastructure.Adapters.Postgres;
using OperatorInterface.Infrastructure.Adapters.Postgres.Repositories;
using OperatorInterface.Queries.UseCases.GetActiveOperatorSession;
using OperatorInterface.Queries.UseCases.GetOperatorSession;
using OperatorInterface.Queries.UseCases.Shared;

var builder = WebApplication.CreateBuilder(args);

// Health Checks
builder.Services.AddHealthChecks();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.ConfigureOptions<SettingsSetup>();
var connectionString = builder.Configuration["CONNECTION_STRING"];

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(connectionString,
            sqlOptions => { sqlOptions.MigrationsAssembly("OperatorInterface.Infrastructure"); });
        options.EnableSensitiveDataLogging();
    }
);

// UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationClient>();
builder.Services.AddScoped<IQueueService, QueueService>();

// Repositories
builder.Services.AddScoped<IOperatorSessionRepository, OperatorSessionRepository>(); 

// MediatR - register from Application layer
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.Lifetime = ServiceLifetime.Scoped;
});

// Commands
builder.Services.AddScoped<IRequestHandler<AssignClientCommand>, AssignClientHandler>();
builder.Services.AddScoped<IRequestHandler<AuthorizeOperatorCommand, SessionId>, AuthorizeOperatorHandler>();
builder.Services.AddScoped<IRequestHandler<CloseOperatorSessionCommand>, CloseOperatorSessionHandler>();
builder.Services.AddScoped<IRequestHandler<CompleteClientSessionCommand>, CompleteClientSessionHandler>();
builder.Services.AddScoped<IRequestHandler<MarkClientAsNotCameCommand>, MarkClientAsNotCameHandler>();
builder.Services.AddScoped<IRequestHandler<OpenOperatorSessionCommand>, OpenOperatorSessionHandler>();
builder.Services.AddScoped<IRequestHandler<RequestClientCommand>, RequestClientHandler>();
builder.Services.AddScoped<IRequestHandler<StartClientSessionCommand>, StartClientSessionHandler>();


// Queries (очень не красиво, подумать)
builder.Services.AddScoped<IRequestHandler<GetActiveOperatorSessionQuery, OperatorSessionDto?>, GetActiveOperatorSessionHandler>();
builder.Services.AddScoped<IRequestHandler<GetOperatorSessionQuery, OperatorSessionDto>, GetOperatorSessionHandler>();


/*
// Channel for event handling (producer-consumer pattern)
builder.Services.AddSingleton<Channel<DomainEvent>>(serviceProvider =>
{
    var options = new BoundedChannelOptions(100)
    {
        FullMode = BoundedChannelFullMode.Wait,
        SingleReader = false,
        SingleWriter = false
    };
    return Channel.CreateBounded<DomainEvent>(options);
});

builder.Services.AddSingleton<ChannelReader<DomainEvent>>(serviceProvider =>
    serviceProvider.GetRequiredService<Channel<DomainEvent>>().Reader);

builder.Services.AddSingleton<ChannelWriter<DomainEvent>>(serviceProvider =>
    serviceProvider.GetRequiredService<Channel<DomainEvent>>().Writer);


// Mock repositories
builder.Services.AddSingleton<Dictionary<SessionId, OperatorSessionDto>>();
builder.Services.AddSingleton<Dictionary<OperatorId, OperatorInfo>>();
*/

// Application services
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<SessionStorageService>();
builder.Services.AddScoped<NotificationService>();

// Background service for event processing
builder.Services.AddHostedService<EventProcessorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Apply Migrations

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseHealthChecks("/health");

// ✅ Простой endpoint для проверки статуса
app.MapGet("/api/status", () => Results.Ok(new 
{ 
    Status = "Running", 
    Version = "2.0", 
    Framework = ".NET 9",
    Environment = app.Environment.EnvironmentName,
    Timestamp = DateTime.UtcNow 
}));

app.Run();

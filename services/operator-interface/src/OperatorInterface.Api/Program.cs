// dotnet ef migrations add Init --startup-project ./OperatorInterface.Api --project ./OperatorInterface.Infrastructure --output-dir ./Adapters/Postgres/Migrations

using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OperatorInterface.Api;
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

// Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin(); // Не делайте так в проде!
        });
});

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

// Mediator (TODO: одумать как от него избавиться)
builder.Services.AddMediatR(cfg =>
{
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

// HTTP Handlers
builder.Services.AddControllers(/*options => { options.InputFormatters.Insert(0, new InputFormatterStream()); */)
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.Converters.Add(new StringEnumConverter
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        });
    });

var app = builder.Build();

// -----------------------------------
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHealthChecks("/health");
app.UseRouting();

app.UseCors();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

// Apply Migrations

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
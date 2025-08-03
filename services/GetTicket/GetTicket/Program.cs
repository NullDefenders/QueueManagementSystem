using GetTicket.Models;
using GetTicket.Services;

var builder = WebApplication.CreateBuilder(args);

// Настройка конфигурации MongoDB
builder.Services.Configure<TicketDatabaseSettings>(
    builder.Configuration.GetSection("TicketDatabaseSettings"));

builder.Services.AddSingleton<GetTicket.Services.TicketService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();

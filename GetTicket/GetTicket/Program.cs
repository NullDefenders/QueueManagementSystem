using GetTicket.Models;
using GetTicket.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TicketDatabaseSettings>(
    builder.Configuration.GetSection("TicketDatabaseSettings"));

builder.Services.AddSingleton<ISimpleRabbitMQSender, SimpleRabbitMQSender>();
builder.Services.AddScoped<TicketService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();
app.Run();

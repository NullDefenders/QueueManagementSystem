using Microsoft.AspNetCore.Http.Features;
using RabbitMqSse.Services;

namespace QueueInformer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Сервисы
            builder.Services.AddControllers();
            builder.Services.AddSingleton<RabbitMQService>();
            builder.Services.AddSingleton<ISseService, SseService>();
            builder.Services.AddHostedService<RabbitMQBackgroundService>();

            // CORS для тестирования
            builder.Services.AddCors(options =>
            {
              options.AddPolicy("AllowAll", policy =>
              {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
              });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Use(async (context, next) =>
            {
                // Полное отключение устранения буфера
                var bufferingFeature = context.Features.Get<IHttpResponseBodyFeature>();
                bufferingFeature?.DisableBuffering();

                context.Response.Headers.Remove("Content-Encoding");
                await next();
            });

            app.Run();
        }
    }
}

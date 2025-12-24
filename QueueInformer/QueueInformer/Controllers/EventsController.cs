using Microsoft.AspNetCore.Mvc;
using QueueInformer.Models;
using QueueInformer.Serveces;
using QueueInformer.Services;
using RabbitMQ.Client;

namespace QueueInformer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ISseService _sseService;
    private readonly RedisService _redisService;

    public EventsController(ISseService sseService, RedisService redisService)
    {
        _sseService = sseService;
        _redisService = redisService;
    }

    [HttpGet]
    public async Task Get()
    {
        Response.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";

        await Response.Body.FlushAsync();

        _sseService.AddClient(Response.Body);
        foreach (var data in await _redisService.GetMessages()) {
            var bytes = System.Text.Encoding.UTF8.GetBytes($"data: {data}\n\n");
            await Response.Body.WriteAsync(bytes, 0, bytes.Length);
            await Response.Body.FlushAsync(); 
        }

        // Ждем, пока клиент не отключится
        try
        {
            await Task.Delay(Timeout.Infinite, HttpContext.RequestAborted);
        }
        catch (TaskCanceledException)
        {
            // Клиент отключился
        }
        finally
        {
            _sseService.RemoveClient(Response.Body);
        }
    }
}
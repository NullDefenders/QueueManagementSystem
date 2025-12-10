using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMqSse.Models;
using RabbitMqSse.Services;

namespace RabbitMqSse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ISseService _sseService;

    public EventsController(ISseService sseService)
    {
        _sseService = sseService;
    }

    [HttpGet]
    public async Task Get()
    {
        Response.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";

        await Response.Body.FlushAsync();

        _sseService.AddClient(Response.Body);

        var data = $"data: Connected to SSE endpoint\n\n";
        var bytes = System.Text.Encoding.UTF8.GetBytes(data);
        await Response.Body.WriteAsync(bytes, 0, bytes.Length);
        await Response.Body.FlushAsync();

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
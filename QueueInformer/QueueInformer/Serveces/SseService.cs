using System.Text;
using QueueInformer.Models;

namespace QueueInformer.Services;

public interface ISseService
{
    void SendToAll(string data);
    void AddClient(Stream writer);
    void RemoveClient(Stream writer);
}

public class SseService : ISseService
{
    private readonly List<Stream> _clients = new();
    private readonly object _clientsLock = new();

    public void AddClient(Stream writer)
    {
        lock (_clientsLock)
        {
            _clients.Add(writer);
        }
    }

    public void RemoveClient(Stream writer)
    {
        lock (_clientsLock)
        {
            _clients.Remove(writer);
        }
    }

    public void SendToAll(string data)
    {
        List<Stream> clientsCopy;
        
        lock (_clientsLock)
        {
            clientsCopy = new List<Stream>(_clients);
        }

        var tasks = clientsCopy.Select(async client =>
        {
            try
            {
                var msg = ($"data: {data}\n\n");
                var bytes = System.Text.Encoding.UTF8.GetBytes(msg);

                await client.WriteAsync(bytes);
                await client.FlushAsync();
            }
            catch
            {
                RemoveClient(client);
            }
        });

        Task.WhenAll(tasks).ContinueWith(t =>
        {
            // Удаляем неактивных клиентов
            var failedClients = clientsCopy.Where(c => !_clients.Contains(c)).ToList();
            lock (_clientsLock)
            {
                foreach (var failedClient in failedClients)
                {
                    _clients.Remove(failedClient);
                }
            }
        });
    }
}
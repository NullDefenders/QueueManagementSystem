public interface IMessageProcessor
{
    Task ProcessAsync(string message);
}
namespace Shared.Interfaces;

public interface IMessageConsumer
{
    Task StartConsumingAsync<T>(string queueName, IMessageHandler<T> handler, CancellationToken cancellationToken = default);
}

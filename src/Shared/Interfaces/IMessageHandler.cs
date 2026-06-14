namespace Shared.Interfaces;

public interface IMessageHandler<in T>
{
    Task HandleAsync(T message, string routingKey, CancellationToken cancellationToken = default);
}

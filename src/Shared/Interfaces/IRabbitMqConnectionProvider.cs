using RabbitMQ.Client;

namespace Shared.Interfaces;

public interface IRabbitMqConnectionProvider
{
    Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
}

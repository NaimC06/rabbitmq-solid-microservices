using RabbitMQ.Client;
using Shared.Interfaces;
using Shared.Settings;

namespace Shared.Messaging;

public sealed class RabbitMqConnectionProvider : IRabbitMqConnectionProvider, IAsyncDisposable
{
    private readonly RabbitMqSettings _settings;
    private IConnection? _connection;

    public RabbitMqConnectionProvider(RabbitMqSettings settings)
    {
        _settings = settings;
    }

    public async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (_connection is not null && _connection.IsOpen)
        {
            return _connection;
        }

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost,
            ClientProvidedName = "rabbitmq-solid-microservices"
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        return _connection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
    }
}

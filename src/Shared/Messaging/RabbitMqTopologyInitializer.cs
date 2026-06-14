using RabbitMQ.Client;
using Shared.Constants;
using Shared.Interfaces;

namespace Shared.Messaging;

public sealed class RabbitMqTopologyInitializer : IRabbitMqTopologyInitializer
{
    private readonly IRabbitMqConnectionProvider _connectionProvider;

    public RabbitMqTopologyInitializer(IRabbitMqConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(ExchangeNames.Direct, ExchangeType.Direct, durable: true, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.ExchangeDeclareAsync(ExchangeNames.Topic, ExchangeType.Topic, durable: true, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.ExchangeDeclareAsync(ExchangeNames.Fanout, ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

        var queueArguments = new Dictionary<string, object?>
        {
            ["x-queue-type"] = "quorum"
        };

        await channel.QueueDeclareAsync(QueueNames.SecurityTeam, durable: true, exclusive: false, autoDelete: false, arguments: queueArguments, cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(QueueNames.Audit, durable: true, exclusive: false, autoDelete: false, arguments: queueArguments, cancellationToken: cancellationToken);

        // Direct: enruta por routing key exacta.
        await channel.QueueBindAsync(QueueNames.SecurityTeam, ExchangeNames.Direct, RoutingKeys.CriticalAlert, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(QueueNames.Audit, ExchangeNames.Direct, RoutingKeys.CriticalAlert, arguments: null, cancellationToken: cancellationToken);

        // Topic: enruta por patrones.
        await channel.QueueBindAsync(QueueNames.SecurityTeam, ExchangeNames.Topic, RoutingKeys.LoginEvents, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(QueueNames.Audit, ExchangeNames.Topic, RoutingKeys.AllSecurityEvents, arguments: null, cancellationToken: cancellationToken);

        // Fanout: envía a todas las colas enlazadas, no importa el routing key.
        await channel.QueueBindAsync(QueueNames.SecurityTeam, ExchangeNames.Fanout, routingKey: string.Empty, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(QueueNames.Audit, ExchangeNames.Fanout, routingKey: string.Empty, arguments: null, cancellationToken: cancellationToken);
    }
}

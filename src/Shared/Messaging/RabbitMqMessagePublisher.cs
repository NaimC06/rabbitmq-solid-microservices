using RabbitMQ.Client;
using Shared.Interfaces;

namespace Shared.Messaging;

public sealed class RabbitMqMessagePublisher : IMessagePublisher
{
    private readonly IRabbitMqConnectionProvider _connectionProvider;
    private readonly IMessageSerializer _serializer;

    public RabbitMqMessagePublisher(IRabbitMqConnectionProvider connectionProvider, IMessageSerializer serializer)
    {
        _connectionProvider = connectionProvider;
        _serializer = serializer;
    }

    public async Task PublishAsync<T>(string exchangeName, string routingKey, T message, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        var body = _serializer.Serialize(message);
        var properties = new BasicProperties
{
    ContentType = "application/json",
    DeliveryMode = DeliveryModes.Persistent
};

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken
        );
    }
}

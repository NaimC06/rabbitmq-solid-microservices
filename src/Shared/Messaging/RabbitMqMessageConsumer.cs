using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Interfaces;

namespace Shared.Messaging;

public sealed class RabbitMqMessageConsumer : IMessageConsumer
{
    private readonly IRabbitMqConnectionProvider _connectionProvider;
    private readonly IMessageSerializer _serializer;

    public RabbitMqMessageConsumer(IRabbitMqConnectionProvider connectionProvider, IMessageSerializer serializer)
    {
        _connectionProvider = connectionProvider;
        _serializer = serializer;
    }

    public async Task StartConsumingAsync<T>(string queueName, IMessageHandler<T> handler, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnectionAsync(cancellationToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
            try
            {
                // Se copia el body antes de procesarlo, buena práctica en RabbitMQ.Client 7.x.
                var body = eventArgs.Body.ToArray();
                var message = _serializer.Deserialize<T>(body);

                await handler.HandleAsync(message, eventArgs.RoutingKey, cancellationToken);
                await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] No se pudo procesar mensaje: {ex.Message}");
                Console.ResetColor();

                await channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false, cancellationToken: cancellationToken);
            }
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
        Console.WriteLine($"Consumidor escuchando cola: {queueName}");
        Console.WriteLine("Presiona CTRL + C para detener.");

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            // Cierre controlado.
        }
        finally
        {
            await channel.DisposeAsync();
        }
    }
}

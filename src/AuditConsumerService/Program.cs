using AuditConsumerService;
using Shared.Constants;
using Shared.Interfaces;
using Shared.Messaging;
using Shared.Models;
using Shared.Serialization;
using Shared.Settings;

using var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellationTokenSource.Cancel();
};

var settings = new RabbitMqSettings();
await using var connectionProvider = new RabbitMqConnectionProvider(settings);
IMessageSerializer serializer = new JsonMessageSerializer();
IRabbitMqTopologyInitializer topologyInitializer = new RabbitMqTopologyInitializer(connectionProvider);
IMessageConsumer consumer = new RabbitMqMessageConsumer(connectionProvider, serializer);
IMessageHandler<SecurityAlert> handler = new AuditAlertHandler();

await topologyInitializer.InitializeAsync(cancellationTokenSource.Token);

Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine("=== AUDIT CONSUMER SERVICE ===");
Console.ResetColor();

await consumer.StartConsumingAsync(QueueNames.Audit, handler, cancellationTokenSource.Token);

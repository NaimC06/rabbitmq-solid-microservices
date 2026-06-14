using Shared.Constants;
using Shared.Interfaces;
using Shared.Messaging;
using Shared.Models;
using Shared.Serialization;
using Shared.Settings;

var settings = new RabbitMqSettings();
await using var connectionProvider = new RabbitMqConnectionProvider(settings);
IMessageSerializer serializer = new JsonMessageSerializer();
IRabbitMqTopologyInitializer topologyInitializer = new RabbitMqTopologyInitializer(connectionProvider);
IMessagePublisher publisher = new RabbitMqMessagePublisher(connectionProvider, serializer);

await topologyInitializer.InitializeAsync();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("=== PRODUCER SERVICE - Gestor de Alertas de Seguridad ===");
Console.ResetColor();
Console.WriteLine("RabbitMQ debe estar activo antes de publicar mensajes.");
Console.WriteLine();

while (true)
{
    Console.WriteLine("Elige un mensaje para publicar:");
    Console.WriteLine("1. DIRECT  - Alerta crítica");
    Console.WriteLine("2. TOPIC   - Login fallido");
    Console.WriteLine("3. TOPIC   - Sistema iniciado");
    Console.WriteLine("4. FANOUT  - Comunicado general");
    Console.WriteLine("0. Salir");
    Console.Write("Opción: ");

    var option = Console.ReadLine();
    Console.WriteLine();

    if (option == "0")
    {
        break;
    }

    var publishRequest = option switch
    {
        "1" => CreatePublishRequest(
            ExchangeNames.Direct,
            RoutingKeys.CriticalAlert,
            "CRITICAL",
            "Se detectó un intento de acceso no autorizado al panel administrativo."
        ),
        "2" => CreatePublishRequest(
            ExchangeNames.Topic,
            RoutingKeys.LoginFailed,
            "MEDIUM",
            "Un usuario falló el inicio de sesión tres veces consecutivas."
        ),
        "3" => CreatePublishRequest(
            ExchangeNames.Topic,
            RoutingKeys.SystemStarted,
            "LOW",
            "El sistema de monitoreo fue iniciado correctamente."
        ),
        "4" => CreatePublishRequest(
            ExchangeNames.Fanout,
            string.Empty,
            "INFO",
            "Comunicado general: mantenimiento programado de seguridad."
        ),
        _ => null
    };

    if (publishRequest is null)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Opción inválida. Intenta otra vez.");
        Console.ResetColor();
        Console.WriteLine();
        continue;
    }

    await publisher.PublishAsync(
        publishRequest.ExchangeName,
        publishRequest.RoutingKey,
        publishRequest.Alert
    );

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"Mensaje publicado en exchange '{publishRequest.ExchangeName}' con routing key '{publishRequest.RoutingKey}'.");
    Console.ResetColor();
    Console.WriteLine();
}

static PublishRequest CreatePublishRequest(string exchangeName, string routingKey, string severity, string message)
{
    var alert = new SecurityAlert(
        Id: Guid.NewGuid(),
        Source: "ProducerService",
        Severity: severity,
        Message: message,
        RoutingKey: routingKey,
        CreatedAtUtc: DateTime.UtcNow
    );

    return new PublishRequest(exchangeName, routingKey, alert);
}

internal sealed record PublishRequest(string ExchangeName, string RoutingKey, SecurityAlert Alert);

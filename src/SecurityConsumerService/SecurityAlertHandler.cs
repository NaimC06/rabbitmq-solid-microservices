using Shared.Interfaces;
using Shared.Models;

namespace SecurityConsumerService;

public sealed class SecurityAlertHandler : IMessageHandler<SecurityAlert>
{
    public Task HandleAsync(SecurityAlert message, string routingKey, CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[SECURITY TEAM] Mensaje recibido");
        Console.ResetColor();
        Console.WriteLine($"ID: {message.Id}");
        Console.WriteLine($"Severidad: {message.Severity}");
        Console.WriteLine($"RoutingKey: {routingKey}");
        Console.WriteLine($"Mensaje: {message.Message}");
        Console.WriteLine($"Fecha UTC: {message.CreatedAtUtc:O}");
        Console.WriteLine(new string('-', 60));

        return Task.CompletedTask;
    }
}

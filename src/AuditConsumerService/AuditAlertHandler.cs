using Shared.Interfaces;
using Shared.Models;

namespace AuditConsumerService;

public sealed class AuditAlertHandler : IMessageHandler<SecurityAlert>
{
    public Task HandleAsync(SecurityAlert message, string routingKey, CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("[AUDIT] Evento registrado para auditoría");
        Console.ResetColor();
        Console.WriteLine($"ID: {message.Id}");
        Console.WriteLine($"Origen: {message.Source}");
        Console.WriteLine($"Severidad: {message.Severity}");
        Console.WriteLine($"RoutingKey: {routingKey}");
        Console.WriteLine($"Mensaje: {message.Message}");
        Console.WriteLine($"Fecha UTC: {message.CreatedAtUtc:O}");
        Console.WriteLine(new string('-', 60));

        return Task.CompletedTask;
    }
}

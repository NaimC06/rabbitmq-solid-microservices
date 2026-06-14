namespace Shared.Models;

public sealed record SecurityAlert(
    Guid Id,
    string Source,
    string Severity,
    string Message,
    string RoutingKey,
    DateTime CreatedAtUtc
);

namespace Shared.Settings;

public sealed class RabbitMqSettings
{
    public string HostName { get; init; } = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
    public string UserName { get; init; } = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
    public string Password { get; init; } = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";
    public int Port { get; init; } = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out var port) ? port : 5672;
    public string VirtualHost { get; init; } = Environment.GetEnvironmentVariable("RABBITMQ_VHOST") ?? "/";
}

using System.Text;
using System.Text.Json;
using Shared.Interfaces;

namespace Shared.Serialization;

public sealed class JsonMessageSerializer : IMessageSerializer
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false
    };

    public byte[] Serialize<T>(T message)
    {
        var json = JsonSerializer.Serialize(message, Options);
        return Encoding.UTF8.GetBytes(json);
    }

    public T Deserialize<T>(byte[] body)
    {
        var json = Encoding.UTF8.GetString(body);
        return JsonSerializer.Deserialize<T>(json, Options)
               ?? throw new InvalidOperationException("No se pudo deserializar el mensaje recibido.");
    }
}

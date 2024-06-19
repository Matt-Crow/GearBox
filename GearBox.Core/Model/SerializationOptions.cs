using System.Text.Json;

namespace GearBox.Core.Model;

public class SerializationOptions
{
    private static readonly JsonSerializerOptions JSON_OPTIONS = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public SerializationOptions()
    {
        JsonSerializerOptions = JSON_OPTIONS;
    }

    public JsonSerializerOptions JsonSerializerOptions { get; init; }
}
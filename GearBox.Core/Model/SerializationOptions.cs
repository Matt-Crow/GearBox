using System.Text.Json;

namespace GearBox.Core.Model;

public class SerializationOptions
{
    private static readonly JsonSerializerOptions JSON_OPTIONS = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public SerializationOptions(bool isWorldInit)
    {
        IsWorldInit = isWorldInit;
        JsonSerializerOptions = JSON_OPTIONS;
    }

    public bool IsWorldInit { get; init; }
    public JsonSerializerOptions JsonSerializerOptions { get; init; }
}
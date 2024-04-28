namespace GearBox.Core.Model.Json;

public readonly struct StableJson : IJson
{
    private StableJson(string? body, bool hasChanged)
    {
        Body = body;
        HasChanged = hasChanged;
    }

    public static StableJson Changed(string body)
    {
        return new StableJson(body, true);
    }

    public static StableJson NoChanges()
    {
        return new StableJson(null, false);
    }

    public string? Body { get; init; }
    public bool HasChanged { get; init; }
}
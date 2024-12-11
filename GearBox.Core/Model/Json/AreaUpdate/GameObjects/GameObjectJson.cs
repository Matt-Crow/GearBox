namespace GearBox.Core.Model.Json.AreaUpdate.GameObjects;

/// <summary>
/// Workaround for https://github.com/dotnet/runtime/issues/31742
/// </summary>
public readonly struct GameObjectJson : IJson
{
    public GameObjectJson(string type, string content)
    {
        Type = type;
        Content = content;
    }

    public string Type { get; init; }
    public string Content { get; init; }
}
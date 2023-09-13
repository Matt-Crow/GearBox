using System.Text.Json;
using System.Text.Json.Serialization;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// A change in a stable dynamic game object
/// </summary>
public readonly struct Change : ISerializable<ChangeJson>
{
    private Change(ChangeType type, IStableGameObject changed)
    {
        Type = type;
        Changed = changed;
    }

    public static Change Created(IStableGameObject created)
    {
        return new Change(ChangeType.Create, created);
    }

    public static Change Updated(IStableGameObject updated)
    {
        return new Change(ChangeType.Update, updated);
    }

    public ChangeType Type { get; init; }
    public IStableGameObject Changed { get; init; }
    public bool IsCreate { get => Type == ChangeType.Create; }
    public bool IsUpdate { get => Type == ChangeType.Update; }

    public ChangeJson ToJson()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { 
                new JsonStringEnumConverter() 
            }
        };
        return new ChangeJson(Type, Changed.Type, Changed.Serialize(options));
    }
}
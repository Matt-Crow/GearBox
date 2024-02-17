using GearBox.Core.Model.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// A change in a stable dynamic game object.
/// A change can be one of two type: Content, or Delete.
/// Content could be either adding or updating.
/// </summary>
public readonly struct Change : ISerializable<ChangeJson>
{
    private Change(IStableGameObject changed, bool isDelete)
    {
        Changed = changed;
        IsDelete = isDelete;
    }

    public static Change Content(IStableGameObject created)
    {
        return new Change(created, false);
    }

    public IStableGameObject Changed { get; init; }
    public bool IsContent => !IsDelete;
    public bool IsDelete { get; init; }

    public ChangeJson ToJson()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        return new ChangeJson(Changed.Type, Changed.Serialize(options), IsDelete);
    }
}
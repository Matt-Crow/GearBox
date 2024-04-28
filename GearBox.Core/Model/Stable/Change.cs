using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// A change in a stable dynamic game object.
/// A change can be one of two type: Content, or Delete.
/// Content could be either adding or updating.
/// </summary>
public readonly struct Change 
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

    public static Change Removed(IStableGameObject removed)
    {
        return new Change(removed, true);
    }

    public IStableGameObject Changed { get; init; }
    public bool IsContent => !IsDelete;
    public bool IsDelete { get; init; }

    public ChangeJson ToJson(bool isWorldInit)
    {
        var asJson = Changed.Serializer?.Serialize(isWorldInit) 
            ?? throw new NotSupportedException("support for non-serializable stable game objects is not yet supported");
        return new ChangeJson(asJson.Type, asJson.Content, IsDelete);
    }
}
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Json;

/// <summary>
/// Describes how a player's UI should look
/// </summary>
public readonly struct UiState
{
    public UiState(PlayerCharacter player)
    {
        OpenShop = player.OpenShop?.GetOpenShopJsonFor(player);
    }

    public OpenShopJson? OpenShop { get; init; }

    public UiStateChangesJson GetChanges(UiState other)
    {
        var result = new UiStateChangesJson()
        {
            OpenShop = CompareNullable(OpenShop, other.OpenShop)
        };
        return result;
    }

    // making this generic is pain - don't try it!
    private static MaybeChangeJson<OpenShopJson?> CompareNullable(OpenShopJson? oldValue, OpenShopJson? newValue)
    {
        if (oldValue == null && newValue == null)
        {
            return MaybeChangeJson<OpenShopJson?>.NoChanges();
        }
        if (oldValue == null || newValue == null)
        {
            return MaybeChangeJson<OpenShopJson?>.Changed(newValue);
        }
        // generics are pain, so I can't delegate to Compare
        if (Hash(oldValue) == Hash(newValue))
        {
            return MaybeChangeJson<OpenShopJson?>.NoChanges();
        }
        return MaybeChangeJson<OpenShopJson?>.Changed(newValue);
    }

    private static MaybeChangeJson<T> Compare<T>(T oldValue, T newValue)
    where T : IChange
    {
        if (Hash(oldValue) == Hash(newValue))
        {
            return MaybeChangeJson<T>.NoChanges();
        }
        return MaybeChangeJson<T>.Changed(newValue);
    }

    private static int Hash(IChange obj)
    {
        var hashCode = new HashCode();
        foreach (var item in obj.DynamicValues)
        {
            hashCode.Add(item);
        }
        return hashCode.ToHashCode();
    }
}
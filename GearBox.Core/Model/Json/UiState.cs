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
        var area = player.CurrentArea ?? player.LastArea;
        Area = area?.ToJson();
        Inventory = player.Inventory.ToJson();
        Weapon = player.WeaponSlot.ToJson();
        Armor = player.ArmorSlot.ToJson();
        Summary = player.StatSummary.ToJson();
        Actives = player.Actives
            .Select(x => new ActiveAbilityJson(x, player))
            .ToList();
        OpenShop = player.OpenShop?.GetOpenShopJsonFor(player);
    }

    public AreaJson? Area { get; init; }
    public InventoryJson Inventory { get; init; }
    public ItemJson? Weapon { get; init;}
    public ItemJson? Armor { get; init;}
    public PlayerStatSummaryJson Summary { get; init; }
    public List<ActiveAbilityJson> Actives { get; init; }
    public OpenShopJson? OpenShop { get; init; }

    /// <summary>
    /// Returns the changes from the old state to the new state
    /// </summary>
    public static UiStateChangesJson GetChanges(UiState? oldState, UiState newState)
    {
        var result = new UiStateChangesJson()
        {
            Area = CompareNullable(oldState?.Area, newState.Area),
            Inventory = Compare(oldState?.Inventory, newState.Inventory),
            Weapon = CompareNullable(oldState?.Weapon, newState.Weapon),
            Armor = CompareNullable(oldState?.Armor, newState.Armor),
            Summary = Compare(oldState?.Summary, newState.Summary),
            Actives = CompareList(oldState?.Actives, newState.Actives),
            OpenShop = CompareNullable(oldState?.OpenShop, newState.OpenShop)
        };
        return result;
    }

    // making this generic is pain - don't try it!
    private static MaybeChangeJson<AreaJson?> CompareNullable(AreaJson? oldValue, AreaJson? newValue)
    {
        if (oldValue == null && newValue == null)
        {
            return MaybeChangeJson<AreaJson?>.NoChanges();
        }
        if (oldValue == null || newValue == null)
        {
            return MaybeChangeJson<AreaJson?>.Changed(newValue);
        }
        // generics are pain, so I can't delegate to Compare
        if (Hash(oldValue) == Hash(newValue))
        {
            return MaybeChangeJson<AreaJson?>.NoChanges();
        }
        return MaybeChangeJson<AreaJson?>.Changed(newValue);
    }

    private static MaybeChangeJson<ItemJson?> CompareNullable(ItemJson? oldValue, ItemJson? newValue)
    {
        if (oldValue == null && newValue == null)
        {
            return MaybeChangeJson<ItemJson?>.NoChanges();
        }
        if (oldValue == null || newValue == null)
        {
            return MaybeChangeJson<ItemJson?>.Changed(newValue);
        }
        // generics are pain, so I can't delegate to Compare
        if (Hash(oldValue) == Hash(newValue))
        {
            return MaybeChangeJson<ItemJson?>.NoChanges();
        }
        return MaybeChangeJson<ItemJson?>.Changed(newValue);
    }

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

    private static MaybeChangeJson<List<T>> CompareList<T>(List<T>? oldValue, List<T> newValue)
    where T : IChange
    {
        if (oldValue == null)
        {
            return MaybeChangeJson<List<T>>.Changed(newValue);
        }
        if (oldValue.Count != newValue.Count)
        {
            return MaybeChangeJson<List<T>>.Changed(newValue);
        }
        
        var anyChanged = oldValue.Zip(newValue)
            .Where(pair => Hash(pair.First) != Hash(pair.Second))
            .Any();
        if (anyChanged)
        {
            return MaybeChangeJson<List<T>>.Changed(newValue);
        }
        return MaybeChangeJson<List<T>>.NoChanges();
    }

    private static MaybeChangeJson<T> Compare<T>(T? oldValue, T newValue)
    where T : IChange
    {
        if (oldValue == null)
        {
            return MaybeChangeJson<T>.Changed(newValue);
        }
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
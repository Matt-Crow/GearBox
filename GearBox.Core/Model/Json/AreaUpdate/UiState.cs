using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Model.Json.AreaUpdate;

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
        Weapon = player.Weapon?.ToJson(1);
        Armor = player.Armor?.ToJson(1);
        Summary = player.StatSummary.ToJson();
        Actives = player.Actives
            .Select(x => new ActiveAbilityJson(x))
            .ToList();
        OpenShop = player.OpenShop?.GetOpenShopJsonFor(player);
    }

    public AreaJson? Area { get; init; }
    public InventoryJson Inventory { get; init; }
    public ItemJson? Weapon { get; init; }
    public ItemJson? Armor { get; init; }
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
        // generics are pain, so I can't delegate to Compare
        if (AreTheSame(oldValue, newValue))
        {
            return MaybeChangeJson<AreaJson?>.NoChanges();
        }
        return MaybeChangeJson<AreaJson?>.Changed(newValue);
    }

    private static MaybeChangeJson<ItemJson?> CompareNullable(ItemJson? oldValue, ItemJson? newValue)
    {
        // generics are pain, so I can't delegate to Compare
        if (AreTheSame(oldValue, newValue))
        {
            return MaybeChangeJson<ItemJson?>.NoChanges();
        }
        return MaybeChangeJson<ItemJson?>.Changed(newValue);
    }

    private static MaybeChangeJson<OpenShopJson?> CompareNullable(OpenShopJson? oldValue, OpenShopJson? newValue)
    {
        // generics are pain, so I can't delegate to Compare
        if (AreTheSame(oldValue, newValue))
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
            .Where(pair => !AreTheSame(pair.First, pair.Second))
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
        if (AreTheSame(oldValue, newValue))
        {
            return MaybeChangeJson<T>.NoChanges();
        }
        return MaybeChangeJson<T>.Changed(newValue);
    }

    private static bool AreTheSame(IChange? oldObj, IChange? newObj)
    {
        /*
            3 cases to consider:
                Are both null?
                Is only one null?
                Are neither null?
        */

        // both are null
        if (oldObj == null && newObj == null)
        {
            return true;
        }

        // only one is null
        if (oldObj == null || newObj == null)
        {
            return false;
        }

        // neither is null

        var oldValues = oldObj.DynamicValues;
        var newValues = newObj.DynamicValues;

        if (oldValues.Count() != newValues.Count())
        {
            return false;
        }

        // check if any of the DynamicValues do not match
        var anyDifferences = oldValues
            .Zip(newValues)
            .Any(pair => !Equals(pair.First, pair.Second)); // Nullable<T>.Equals() method
        return !anyDifferences;
    }
}
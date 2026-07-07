namespace GearBox.Core.Model.Items;

public class LootOption
{
    private readonly ItemUnion? _item;
    private readonly Gold? _gold;

    public LootOption(ItemUnion item)
    {
        Weight = item.Grade.Weight;
        _item = item;
    }

    public LootOption(Grade grade, Gold gold)
    {
        Weight = grade.Weight;
        _gold = gold;
    }


    public int Weight { get; init; }

    public void Match(Action<ItemUnion> withItem, Action<Gold> withGold)
    {
        if (_item != null)
        {
            withItem(_item);
        }
        else if (_gold != null)
        {
            withGold(_gold);
        }
        else
        {
            throw new Exception($"Missing case in {nameof(Match)}");
        }
    }

    public T Select<T>(Func<ItemUnion, T> withItem, Func<Gold, T> withGold)
    {
        if (_item != null)
        {
            return withItem(_item);
        }
        if (_gold != null)
        {
            return withGold(_gold);
        }
        throw new Exception($"Missing case in {nameof(Select)}");
    }
}
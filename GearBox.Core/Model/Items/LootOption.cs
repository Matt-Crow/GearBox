namespace GearBox.Core.Model.Items;

public class LootOption
{
    public LootOption(int weight, ItemUnion item)
    {
        Weight = weight;
        Item = item;
    }

    public LootOption(int weight, Gold gold)
    {
        Weight = weight;
        Gold = gold;
    }

    public int Weight { get; init; }
    public ItemUnion? Item { get; init; }
    public Gold? Gold { get; init; }
}
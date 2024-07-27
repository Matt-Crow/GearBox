namespace GearBox.Core.Model.Items;

/// <summary>
/// Used to determine what items players can get from LootChests
/// </summary>
public class LootTable
{
    private readonly List<LootOption> _lootOptions = [];

    public void AddWeapon(Weapon itemDefinition) => _lootOptions.Add(new(itemDefinition.Type.Grade, ItemUnion.Of(itemDefinition)));

    public void AddArmor(Armor itemDefinition) => _lootOptions.Add(new(itemDefinition.Type.Grade, ItemUnion.Of(itemDefinition)));

    public void AddMaterial(Material itemDefinition) => _lootOptions.Add(new(itemDefinition.Type.Grade, ItemUnion.Of(itemDefinition)));
    public LootTable AddGold(Grade grade, Gold gold)
    {
        _lootOptions.Add(new(grade, gold));
        return this;
    }

    public Inventory GetRandomLoot()
    {
        var result = new Inventory();
        var numItems = _lootOptions.Any()
            ? Random.Shared.Next(0, 3) + 1
            : 0;
        for (int i = 0; i < numItems; i++)
        {
            var itemToAdd = ChooseRandomWeightedItem();
            result.Add(itemToAdd.Item?.ToOwned());
            result.Add(itemToAdd.Gold);
        }

        return result;
    }

    private LootOption ChooseRandomWeightedItem()
    {
        var totalWeight = _lootOptions.Sum(x => x.Weight);
        var randomNumber = Random.Shared.Next(totalWeight);
        foreach (var weightedItem in _lootOptions)
        {
            if (weightedItem.Weight > randomNumber)
            {
                return weightedItem;
            }
            randomNumber -= weightedItem.Weight;
        }
        throw new Exception("Something went wrong when chosing a random item");
    }

    private class LootOption
    {
        public LootOption(Grade grade, ItemUnion item)
        {
            Weight = grade.Weight;
            Item = item;
        }

        public LootOption(Grade grade, Gold gold)
        {
            Weight = grade.Weight;
            Gold = gold;
        }

        public int Weight { get; init; }
        public ItemUnion? Item { get; init; }
        public Gold? Gold { get; init; }
    }
}
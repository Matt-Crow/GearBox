namespace GearBox.Core.Model.Items;

/// <summary>
/// Used to determine what items players can get from LootChests
/// </summary>
public class LootTable
{
    private readonly List<WeightedItem> _weightedItems = [];

    public void AddWeapon(Weapon itemDefinition) => _weightedItems.Add(new(itemDefinition.Type.Grade.Weight, ItemUnion.Of(itemDefinition)));

    public void AddArmor(Armor itemDefinition) => _weightedItems.Add(new(itemDefinition.Type.Grade.Weight, ItemUnion.Of(itemDefinition)));

    public void AddMaterial(Material itemDefinition) => _weightedItems.Add(new(itemDefinition.Type.Grade.Weight, ItemUnion.Of(itemDefinition)));

    public Inventory GetRandomLoot()
    {
        var result = new Inventory();
        var numItems = _weightedItems.Any()
            ? Random.Shared.Next(0, 3) + 1
            : 0;
        for (int i = 0; i < numItems; i++)
        {
            var itemToAdd = ChooseRandomWeightedItem();
            result.Add(itemToAdd.Item.ToOwned());
        }

        // todo read from loot pool
        var goldAmount = Random.Shared.Next(0, 20) + 1;
        result.Add(new Gold(goldAmount));

        return result;
    }

    private WeightedItem ChooseRandomWeightedItem()
    {
        var totalWeight = _weightedItems.Sum(x => x.Weight);
        var randomNumber = Random.Shared.Next(totalWeight);
        foreach (var weightedItem in _weightedItems)
        {
            if (weightedItem.Weight > randomNumber)
            {
                return weightedItem;
            }
            randomNumber -= weightedItem.Weight;
        }
        throw new Exception("Something went wrong when chosing a random item");
    }

    private class WeightedItem
    {
        public WeightedItem(int weight, ItemUnion item)
        {
            Weight = weight;
            Item = item;
        }

        public int Weight { get; init; }
        public ItemUnion Item { get; init; }
    }
}
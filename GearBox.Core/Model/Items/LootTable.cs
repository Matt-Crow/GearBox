using GearBox.Core.Utils;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Used to determine what items players can get from LootChests
/// </summary>
public class LootTable
{
    private readonly List<LootOption> _lootOptions = [];
    private readonly IRandomNumberGenerator _rng;

    public LootTable(List<LootOption> options, IRandomNumberGenerator rng)
    {
        _lootOptions = options;
        _rng = rng;
    }

    public Inventory GetRandomLoot()
    {
        var result = new Inventory();
        var numItems = _lootOptions.Any()
            ? _rng.Next(0, 3) + 1
            : 0;
        for (int i = 0; i < numItems; i++)
        {
            ChooseRandomLootOption()
                .Match(
                    item => result.Add(item.ToOwned()),
                    gold => result.Add(gold)
                );
        }
        return result;
    }

    private LootOption ChooseRandomLootOption()
    {
        var totalWeight = _lootOptions.Sum(x => x.Weight);
        var randomNumber = _rng.Next(totalWeight);
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
}
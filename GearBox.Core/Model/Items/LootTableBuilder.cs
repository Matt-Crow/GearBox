using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model.Items;

public class LootTableBuilder
{
    private readonly IItemFactory _itemFactory;
    private readonly List<LootOption> _lootOptions = [];

    public LootTableBuilder(IItemFactory itemFactory)
    {
        _itemFactory = itemFactory;
    }

    public LootTableBuilder AddItem(string name)
    {
        var item = _itemFactory.Make(name) ?? throw new ArgumentException($"Bad item name: '{name}'");
        _lootOptions.Add(new LootOption(item.Grade.Weight, item)); 
        return this;
    }

    public LootTableBuilder Add(Grade grade, Gold gold)
    {
        _lootOptions.Add(new LootOption(grade.Weight, gold));
        return this;
    }

    public LootTable Build(int level)
    {
        var lootOptions = _lootOptions
            .Select(opt => opt.Select(
                item => new LootOption(item.Grade.Weight, item.ToOwned(level)),
                gold => opt
            ))
            .ToList();
        var result = new LootTable(lootOptions);
        return result;
    }
}
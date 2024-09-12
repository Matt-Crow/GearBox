using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model.Items;

public class LootTableBuilder
{
    private readonly IItemFactory _itemFactory;
    private readonly List<LootOptionBuilder> _lootOptions = [];

    public LootTableBuilder(IItemFactory itemFactory)
    {
        _itemFactory = itemFactory;
    }

    public LootTableBuilder AddItem(string name)
    {
        _lootOptions.Add(new(name));
        return this;
    }

    public LootTableBuilder Add(Grade grade, Gold gold)
    {
        _lootOptions.Add(new(grade, gold));
        return this;
    }

    public LootTable Build(int level)
    {
        var lootOptions = _lootOptions
            .Select(x => x.BuildUsing(_itemFactory, level))
            .ToList();
        var result = new LootTable(lootOptions);
        return result;
    }

    private class LootOptionBuilder
    {
        public LootOptionBuilder(Grade grade, Gold gold)
        {
            Grade = grade;
            Gold = gold;
        }

        public LootOptionBuilder(string itemName)
        {
            ItemName = itemName;
        }

        public Grade? Grade { get; init; }
        public Gold? Gold { get; init; }
        public string? ItemName { get; init; }

        public LootOption BuildUsing(IItemFactory itemFactory, int level)
        {
            if (Grade != null && Gold != null)
            {
                return new LootOption(Grade.Value.Weight, Gold.Value);
            }
            if (ItemName != null)
            {
                var item = itemFactory.Make(ItemName) ?? throw new Exception($"Bad item name: '{ItemName}'");
                return new LootOption(item.Grade.Weight, item.ToOwned(level));
            }
            throw new Exception("Missing case in LootOptionBuilder.BuildUsing(IItemFactory)");
        }
    }
}
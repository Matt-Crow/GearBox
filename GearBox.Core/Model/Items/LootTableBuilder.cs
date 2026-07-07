using GearBox.Core.Utils;

namespace GearBox.Core.Model.Items;

public class LootTableBuilder
{
    private readonly IRandomNumberGenerator _rng;
    private readonly List<LootOption> _lootOptions = [];


    public LootTableBuilder(IRandomNumberGenerator rng)
    {
        _rng = rng;
    }

    
    public LootTableBuilder AddOption(LootOption option)
    {
        _lootOptions.Add(option);
        return this;
    }

    public LootTable Build(int level)
    {
        var lootOptions = _lootOptions
            .Select(opt => opt.Select(
                item => new LootOption(item.ToOwned(level)),
                gold => opt
            ))
            .ToList();
        var result = new LootTable(lootOptions, _rng);
        return result;
    }
}
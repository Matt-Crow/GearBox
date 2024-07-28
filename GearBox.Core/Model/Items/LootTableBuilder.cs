namespace GearBox.Core.Model.Items;

public class LootTableBuilder
{
    // todo will need some sort of item repository from the game builder
    
    private readonly List<LootOption> _lootOptions = [];

    public LootTableBuilder Add(Grade grade, Material material)
    {
        _lootOptions.Add(new(grade.Weight, ItemUnion.Of(material)));
        return this;
    }

    public LootTableBuilder Add(Grade grade, EquipmentBuilder<Weapon> weapon, int level)
    {
        _lootOptions.Add(new(grade.Weight, ItemUnion.Of(weapon.Build(level))));
        return this;
    }

    public LootTableBuilder Add(Grade grade, EquipmentBuilder<Armor> armor, int level)
    {
        _lootOptions.Add(new(grade.Weight, ItemUnion.Of(armor.Build(level))));
        return this;
    }

    public LootTableBuilder Add(Grade grade, Gold gold)
    {
        _lootOptions.Add(new(grade.Weight, gold));
        return this;
    }

    public LootTable Build()
    {
        var result = new LootTable(_lootOptions);
        return result;
    }
}
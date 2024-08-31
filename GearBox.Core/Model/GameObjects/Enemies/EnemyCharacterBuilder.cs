using GearBox.Core.Model.Items;

namespace GearBox.Core.Model.GameObjects.Enemies;

public class EnemyCharacterBuilder
{
    private readonly string _name;
    private readonly Color _color;
    private readonly LootTableBuilder _loot;

    public EnemyCharacterBuilder(string name, Color color, LootTableBuilder loot)
    {
        _name = name;
        _color = color;
        _loot = loot;
    }

    public EnemyCharacter Build(int level)
    {
        var result = new EnemyCharacter(_name, level, _color, _loot.Build(level));
        return result;
    }
}
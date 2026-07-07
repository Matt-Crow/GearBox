using GearBox.Core.Model.Items;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.GameObjects.Enemies;

public class EnemyRepository : IEnemyRepository
{
    private readonly IRandomNumberGenerator _rng;
    private readonly Dictionary<string, EnemyCharacterBuilder> _enemyBuilders = [];


    public EnemyRepository(IRandomNumberGenerator rng)
    {
        _rng = rng;
    }

    public IEnemyRepository Add(EnemyCharacterTemplate enemy)
    {
        var lootTableBuilder = new LootTableBuilder(_rng);
        foreach (var option in enemy.LootOptions)
        {
            lootTableBuilder = lootTableBuilder.AddOption(option);
        }
        var enemyBuilder = new EnemyCharacterBuilder(enemy.Name, enemy.Color, lootTableBuilder);
        _enemyBuilders[enemy.Name] = enemyBuilder;
        return this;
    }


    public EnemyCharacter? GetEnemyByName(string name, int level)
    {
        if (_enemyBuilders.TryGetValue(name, out EnemyCharacterBuilder? value))
        {
            return value.Build(level);
        }
        return null;
    }
}
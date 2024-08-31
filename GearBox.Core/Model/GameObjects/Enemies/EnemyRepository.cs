using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model.GameObjects.Enemies;

public class EnemyRepository : IEnemyRepository
{
    private readonly IItemFactory _itemFactory;
    private readonly Dictionary<string, EnemyCharacterBuilder> _enemyBuilders = [];

    public EnemyRepository(IItemFactory itemFactory)
    {
        _itemFactory = itemFactory;
    }

    public IEnemyRepository Add(string name, Color color, Func<LootTableBuilder, LootTableBuilder> loot)
    {
        var lootTableBuilder = new LootTableBuilder(_itemFactory);
        var enemyBuilder = new EnemyCharacterBuilder(name, color, loot(lootTableBuilder));
        _enemyBuilders[name] = enemyBuilder;
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
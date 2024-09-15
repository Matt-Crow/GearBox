using GearBox.Core.Model.Items;

namespace GearBox.Core.Model.GameObjects.Enemies;

/// <summary>
/// Defines all the different types of enemies a player may encounter in the game.
/// </summary>
public interface IEnemyRepository
{
    IEnemyRepository Add(string name, Color color, Func<LootTableBuilder, LootTableBuilder> loot);
    
    EnemyCharacter? GetEnemyByName(string name, int level);
}
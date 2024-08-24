namespace GearBox.Core.Model.GameObjects.Enemies;

/// <summary>
/// Defines all the different types of enemies a player may encounter in the game.
/// </summary>
public interface IEnemyRepository
{
    IEnemyRepository Add(EnemyCharacter enemy);
    EnemyCharacter? GetEnemyByName(string name);
}
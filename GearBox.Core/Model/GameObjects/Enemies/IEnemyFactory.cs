namespace GearBox.Core.Model.GameObjects.Enemies;

public interface IEnemyFactory
{
    /// <summary>
    /// Looks up the enemy with the given name in the backing store, then adds it to this, or throws an error if not found
    /// </summary>
    IEnemyFactory Add(string name);

    EnemyCharacter? MakeRandom(int level);
}
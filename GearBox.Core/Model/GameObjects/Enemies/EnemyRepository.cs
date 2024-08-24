namespace GearBox.Core.Model.GameObjects.Enemies;

public class EnemyRepository : IEnemyRepository
{
    private readonly Dictionary<string, EnemyCharacter> _enemies = [];

    public IEnemyRepository Add(EnemyCharacter enemy)
    {
        _enemies[enemy.Name] = enemy;
        return this;
    }

    public EnemyCharacter? GetEnemyByName(string name)
    {
        if (_enemies.TryGetValue(name, out EnemyCharacter? value))
        {
            return value;
        }
        return null;
    }
}
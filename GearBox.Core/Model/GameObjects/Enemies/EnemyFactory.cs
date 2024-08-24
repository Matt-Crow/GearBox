namespace GearBox.Core.Model.GameObjects.Enemies;

public class EnemyFactory : IEnemyFactory
{
    private readonly IEnemyRepository _allEnemies;
    private readonly List<string> _names = [];

    public EnemyFactory(IEnemyRepository allEnemies)
    {
        _allEnemies = allEnemies;
    }

    public IEnemyFactory Add(string name)
    {
        var _ = _allEnemies.GetEnemyByName(name) ?? throw new ArgumentException($"Bad enemy name: {name}", nameof(name));
        _names.Add(name);
        return this;
    }

    public EnemyCharacter? MakeRandom(int level)
    {
        if (!_names.Any())
        {
            return null;
        }
        var name = _names[Random.Shared.Next(_names.Count)];
        var result = _allEnemies.GetEnemyByName(name) ?? throw new Exception($"Bad enemy name: {name}");
        return result.ToOwned(level);
    }
}
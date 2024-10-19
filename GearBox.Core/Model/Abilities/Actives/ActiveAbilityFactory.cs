namespace GearBox.Core.Model.Abilities.Actives;

public class ActiveAbilityFactory : IActiveAbilityFactory
{
    private readonly Dictionary<string, IActiveAbility> _actives = [];

    public IActiveAbilityFactory Add(IActiveAbility active)
    {
        if (_actives.ContainsKey(active.Name))
        {
            throw new ArgumentException($"Duplicate active ability name: \"{active.Name}\"", nameof(active));
        }
        _actives[active.Name] = active;
        return this;
    }

    public IActiveAbility? Make(string name)
    {
        _actives.TryGetValue(name, out var active);
        var result = active?.Copy();
        return result;
    }
}

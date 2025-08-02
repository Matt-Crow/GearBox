namespace GearBox.Core.Model.Abilities.Passives;

public class PassiveAbilityFactory : IPassiveAbilityFactory
{
    private readonly Dictionary<string, IPassiveAbility> _passives = [];

    public IPassiveAbilityFactory Add(IPassiveAbility passive)
    {
        if (_passives.ContainsKey(passive.Name))
        {
            throw new ArgumentException($"Duplicate passive ability name: \"{passive.Name}\"", nameof(passive));
        }
        _passives[passive.Name] = passive;
        return this;
    }

    public IPassiveAbility? Make(string name)
    {
        _passives.TryGetValue(name, out var passive);
        var result = passive?.Copy();
        return result;
    }
}
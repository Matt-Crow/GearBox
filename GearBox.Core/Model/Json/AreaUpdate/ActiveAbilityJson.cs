using GearBox.Core.Model.Abilities.Actives;

namespace GearBox.Core.Model.Json.AreaUpdate;

public class ActiveAbilityJson : IChange
{
    public ActiveAbilityJson(IActiveAbility ability)
    {
        Name = ability.Name;
        EnergyCost = ability.EnergyCost;
        SecondsUntilNextUse = (int)ability.TimeUntilNextUse.InSeconds;
        CanBeUsed = ability.CanBeUsed();
        Description = ability.GetDescription();
    }

    public string Name { get; init; }
    public int EnergyCost { get; init; }
    public int SecondsUntilNextUse { get; init; }
    public bool CanBeUsed { get; init; }
    public string Description { get; init; }

    // Name & EnergyCost don't change, but need Name to notice when player changes actives
    public IEnumerable<object?> DynamicValues => [Name, SecondsUntilNextUse, CanBeUsed];
}

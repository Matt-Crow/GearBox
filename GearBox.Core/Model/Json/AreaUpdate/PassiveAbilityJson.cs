using GearBox.Core.Model.Abilities.Passives;

namespace GearBox.Core.Model.Json.AreaUpdate;

public class PassiveAbilityJson : IChange
{
    public PassiveAbilityJson(IPassiveAbility ability)
    {
        Name = ability.Name;
        Description = ability.GetDescription();
    }

    public string Name { get; init; }
    public string Description { get; init; }

    // Name doesn't change, but need Name to notice when player changes passives
    public IEnumerable<object?> DynamicValues => [Name];
}
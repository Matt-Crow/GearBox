namespace GearBox.Core.Model.Abilities.Passives;

/// <summary>
/// Makes passive abilities
/// </summary>
public interface IPassiveAbilityFactory
{
    /// <summary>
    /// Registers the given passive ability so it can be made later.
    /// It is an error to add multiple passive abilities with the same name.
    /// </summary>
    IPassiveAbilityFactory Add(IPassiveAbility passive);

    /// <summary>
    /// Returns a copy of the registered passive ability with the given name,
    /// or null if no such passive ability exists.
    /// </summary>
    IPassiveAbility? Make(string name);
}
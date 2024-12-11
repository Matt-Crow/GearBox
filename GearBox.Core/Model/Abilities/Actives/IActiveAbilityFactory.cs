namespace GearBox.Core.Model.Abilities.Actives;

/// <summary>
/// Makes active abilities
/// </summary>
public interface IActiveAbilityFactory
{
    /// <summary>
    /// Registers the given active ability so it can be made later.
    /// It is an error to add multiple active abilities with the same name.
    /// </summary>
    IActiveAbilityFactory Add(IActiveAbility active);

    /// <summary>
    /// Returns a copy of the registered active ability with the given name,
    /// or null if no such active ability exists.
    /// </summary>
    IActiveAbility? Make(string name);
}
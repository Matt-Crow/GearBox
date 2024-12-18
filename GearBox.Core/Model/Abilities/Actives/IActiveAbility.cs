using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives;

/// <summary>
/// An ability players can actively choose when to trigger
/// </summary>
public interface IActiveAbility
{
    /// <summary>
    /// Must be unique within a game
    /// </summary>
    string Name { get; }

    /// <summary>
    /// How must energy it costs a player to trigger this ability
    /// </summary>
    int EnergyCost { get; }

    /// <summary>
    /// How long the user must wait to use this ability after it is triggered
    /// </summary>
    Duration Cooldown { get; }

    /// <summary>
    /// How long until this ability can be triggered again
    /// </summary>
    Duration TimeUntilNextUse { get; }

    /// <summary>
    /// The character who this ability belongs to, if any
    /// </summary>
    Character? User { get; set; }

    /// <summary>
    /// Checks whether the current user can use this ability right now
    /// </summary>
    bool CanBeUsed();

    /// <summary>
    /// Triggers this active ability, if able
    /// </summary>
    void Use(Direction inDirection); 

    /// <summary>
    /// Gets a short discription to display to the player
    /// </summary>
    string GetDescription();

    /// <summary>
    /// Called at the end of each frame
    /// </summary>
    void Update();

    /// <summary>
    /// Returns a deep copy of this
    /// </summary>
    IActiveAbility Copy();
}
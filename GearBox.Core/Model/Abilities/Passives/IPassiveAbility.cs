using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Model.Abilities.Passives;

/// <summary>
/// A player ability which is passively triggered by events 
/// </summary>
public interface IPassiveAbility
{
    /// <summary>
    /// Must be unique within a game
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The character who this ability belongs to, if any
    /// </summary>
    Character? User { get; }

    /// <summary>
    /// Un-registers event listeners from the old user,
    /// then registers event listeners on the new user
    /// </summary>
    void SetUser(Character? user);

    /// <summary>
    /// Gets a short description to display to the player
    /// </summary>
    string GetDescription();

    /// <summary>
    /// Called at the end of each frame
    /// </summary>
    void Update();

    /// <summary>
    /// Returns a deep copy of this
    /// </summary>
    IPassiveAbility Copy();
}
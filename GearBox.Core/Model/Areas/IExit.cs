using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Model.Areas;

/// <summary>
/// An exit exists in an area, and moves players to another area when they pass through it.
/// </summary>
public interface IExit
{
    /// <summary>
    /// The name of the area this sends players to when they reach this exits
    /// </summary>
    string DestinationName { get; }

    /// <summary>
    /// Checks whether the given player should exit the given area
    /// </summary>
    bool ShouldExit(PlayerCharacter player, IArea area);

    /// <summary>
    /// Run after the given player leaves the given area.
    /// For example, this could move the player to the top of the new area.
    /// </summary>
    void OnExit(PlayerCharacter player, IArea area);
}
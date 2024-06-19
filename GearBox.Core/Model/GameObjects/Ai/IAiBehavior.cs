namespace GearBox.Core.Model.GameObjects.Ai;

/// <summary>
/// Controls a character
/// </summary>
public interface IAiBehavior
{
    /// <summary>
    /// Applies the AI behavior.
    /// setBehavior may be called by this method to update the target's state
    /// </summary>
    void Update();
}
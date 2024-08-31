namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// Killed is not the same as terminated:
/// Termination is when any game object should be removed from the game.
/// Killed is when a character takes too much damage and must be terminated.
/// </summary>
public class KilledEventArgs : EventArgs
{
    public KilledEventArgs(AttackedEventArgs attackEvent)
    {
        AttackEvent = attackEvent;
    }
    
    /// <summary>
    /// The attack event which killed
    /// </summary>
    public AttackedEventArgs AttackEvent { get; init; }
}
namespace GearBox.Core.Model.GameObjects;

public class AttackedEventArgs : EventArgs
{
    public AttackedEventArgs(Attack attackUsed, Character whoWasAttacked)
    {
        AttackUsed = attackUsed;
        WhoWasAttacked = whoWasAttacked;
    }

    public Attack AttackUsed { get; init; }
    public Character WhoWasAttacked { get; init; }
}
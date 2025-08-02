namespace GearBox.Core.Model.GameObjects;

public class AttackedEvent
{
    public AttackedEvent(Attack attackUsed, Character whoWasAttacked)
    {
        AttackUsed = attackUsed;
        WhoWasAttacked = whoWasAttacked;
    }

    public Attack AttackUsed { get; init; }
    public Character WhoWasAttacked { get; init; }
}
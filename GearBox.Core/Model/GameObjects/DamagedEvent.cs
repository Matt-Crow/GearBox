namespace GearBox.Core.Model.GameObjects;

public class DamagedEvent
{
    public DamagedEvent(Character whoWasDamaged, int damage)
    {
        WhoWasDamaged = whoWasDamaged;
        Damage = damage;    
    }

    
    public Character WhoWasDamaged { get; init; }
    public int Damage { get; init; }


    /// <summary>
    /// Returns a copy of this event, but with the given damage.
    /// </summary>
    public DamagedEvent WithDamage(int damage)
    {
        var result = new DamagedEvent(WhoWasDamaged, damage);
        return result;
    }
}
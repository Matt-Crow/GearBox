namespace GearBox.Core.Model.GameObjects;

public class Attack
{
    private readonly HashSet<Character> _collidedWith = [];

    public Attack(Character usedBy, int damage)
    {
        UsedBy = usedBy;
        Damage = damage;
    }

    public Character UsedBy { get; init; }
    public int Damage { get; init; }

    public bool CanResolveAgainst(Character target)
    {
        return target != UsedBy && !_collidedWith.Contains(target) && target.Team != UsedBy.Team;
    }

    public void HandleCollision(object? sender, CollideEventArgs args)
    {
        var target = args.CollidedWith;

        if (CanResolveAgainst(target))
        {
            _collidedWith.Add(target);
            var attackEvent = new AttackedEvent(this, target);
            target.HandleAttacked(attackEvent);
        }
    }
}
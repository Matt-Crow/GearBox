namespace GearBox.Core.Model.Dynamic;

public class Attack
{
    private readonly Character _usedBy;
    private readonly HashSet<Character> _collidedWith = [];
    private readonly int _damage;

    public Attack(Character usedBy, int damage)
    {
        _usedBy = usedBy;
        _damage = damage;
    }

    public bool CanResolveAgainst(Character target)
    {
        return target != _usedBy && !_collidedWith.Contains(target);
    }

    public void HandleCollision(object? sender, CollideEventArgs args)
    {
        // in the future, this will use Teams as well
        var target = args.CollidedWith;

        if (CanResolveAgainst(target))
        {
            target.TakeDamage(_damage);
            _collidedWith.Add(target);
        }
    }
}
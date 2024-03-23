namespace GearBox.Core.Model.Dynamic;

public class Attack
{
    private readonly Character _usedBy;

    public Attack(Character usedBy)
    {
        _usedBy = usedBy;
    }

    public void HandleCollision(object? sender, CollideEventArgs args)
    {
        // in the future, this will use Teams as well
        if (args.CollidedWith != _usedBy)
        {
            Console.WriteLine("collided");
        }
    }
}
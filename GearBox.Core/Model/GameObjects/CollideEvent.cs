namespace GearBox.Core.Model.GameObjects;

public class CollideEvent
{
    public CollideEvent(Character collidedWith) => CollidedWith = collidedWith;
    
    public Character CollidedWith { get; init; }
}
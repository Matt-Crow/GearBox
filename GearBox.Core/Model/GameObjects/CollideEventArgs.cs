namespace GearBox.Core.Model.GameObjects;

public class CollideEventArgs : EventArgs
{
    public CollideEventArgs(Character collidedWith) => CollidedWith = collidedWith;
    
    public Character CollidedWith { get; init; }
}
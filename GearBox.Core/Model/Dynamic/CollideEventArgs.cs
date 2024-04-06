namespace GearBox.Core.Model.Dynamic;

public class CollideEventArgs : EventArgs
{
    public CollideEventArgs(Character collidedWith) => CollidedWith = collidedWith;
    
    public Character CollidedWith { get; init; }
}
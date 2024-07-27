namespace GearBox.Core.Model.Items;

public readonly struct Gold
{
    public static readonly Gold NONE = new(0);

    public Gold(int quantity)
    {
        Quantity = quantity;
    }

    public int Quantity { get; init; }

    public Gold Plus(Gold other) => new(Quantity + other.Quantity);
}
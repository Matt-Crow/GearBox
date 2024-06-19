namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerStat
{
    public int Points { get; set; } = 0;
    public double Value => (double)Points / 100;
    public IEnumerable<object?> DynamicValues => [Points];
}
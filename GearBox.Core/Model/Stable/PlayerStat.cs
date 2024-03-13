namespace GearBox.Core.Model.Stable;

public class PlayerStat<T>
{
    private readonly Func<int, T> _formula;

    private PlayerStat(Func<int, T> formula)
    {
        _formula = formula;
    }

    public static PlayerStat<int> Linear(int start, double step) => new(x => start + (int)(step*x));
    public int Points { get; set; } = 0;
    public T Value => _formula(Points);
    public IEnumerable<object?> DynamicValues => new List<object?>()
    {
        Points
    };
}
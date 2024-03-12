using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

public class Fraction : ISerializable<FractionJson>
{
    public Fraction(int current, int max)
    {
        Current = current;
        Max = max;
    }

    public int Current { get; private set; }
    public int Max { get; private set; } // max can change e.g. when player changes armor
    
    public void RestorePercent(double percent)
    {
        Current += (int)(percent * Max);
        if (Current > Max)
        {
            Current = Max;
        }
    }

    public IEnumerable<object?> DynamicValues => new List<object?>()
    {
        Current,
        Max
    };

    public FractionJson ToJson() => new FractionJson(Current, Max);
}
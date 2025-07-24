namespace GearBox.Core.Utils;

public class RandomNumberGenerator : IRandomNumberGenerator
{
    public int Next(int max) => Random.Shared.Next(max);

    public bool CheckChance(double percentChance)
    {
        if (percentChance == 0.0)
        {
            return false;
        }
        if (percentChance == 1.0)
        {
            return true;
        }
        if (percentChance > 1.0 || percentChance < 0.0)
        {
            throw new ArgumentException($"{nameof(percentChance)} should be between 0 and 1 (inclusive)");
        }
        return Random.Shared.NextDouble() < percentChance;
    }

    public T ChooseRandom<T>(List<T> options)
    {
        if (options.Count == 0)
        {
            throw new ArgumentException($"{nameof(options)} cannot be empty");
        }

        var index = Next(options.Count);
        var choice = options[index];
        return choice;
    }
}
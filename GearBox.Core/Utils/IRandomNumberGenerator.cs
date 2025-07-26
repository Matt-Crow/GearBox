namespace GearBox.Core.Utils;

public interface IRandomNumberGenerator
{
    /// <summary>
    /// Returns a non-negative int less than max
    /// </summary>
    int Next(int max);

    /// <summary>
    /// Returns a random integer that is within a specified range.
    /// </summary>
    int Next(int minValue, int maxValue);

    /// <summary>
    /// For example, CheckChance(0.33) returns true 33% of the time
    /// </summary>
    bool CheckChance(double percentChance);

    /// <summary>
    /// Returns a random option from the given options
    /// </summary>
    T ChooseRandom<T>(List<T> options);
}
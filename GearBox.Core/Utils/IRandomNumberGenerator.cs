namespace GearBox.Core.Utils;

public interface IRandomNumberGenerator
{
    /// <summary>
    /// For example, CheckChance(0.33) returns true 33% of the time
    /// </summary>
    bool CheckChance(double percentChance);

    /// <summary>
    /// Returns a random option from the given options
    /// </summary>
    T ChooseRandom<T>(List<T> options);
}
using GearBox.Core.Model.Units;

namespace GearBox.Core.Config;

public class EnemySpawnConfig
{
    /// <summary>
    /// The maximum number of enemies which can exist in an area at one time
    /// </summary>
    public int MaxEnemies { get; set; } = 10;

    /// <summary>
    /// The number of seconds between each wave of enemies
    /// </summary>
    public int PeriodInSeconds { get; set; } = 10;

    /// <summary>
    /// The number of enemies which spawn each wave
    /// </summary>
    public int WaveSize { get; set; } = 3;

    /// <summary>
    /// The number of frames between each wave of enemies
    /// </summary>
    public int PeriodInFrames => Duration.FromSeconds(PeriodInSeconds).InFrames;
}
namespace GearBox.Core.Config;

public class GearBoxConfig
{
    /// <summary>
    /// When set, enemies will spawn with no AI attached.
    /// </summary>
    public bool DisableAI { get; set; } = false;

    /// <summary>
    /// Have much XP enemies will drop relative to their level.
    /// For example, a multiplier of 10 means a level 2 enemy will drop 20 XP.
    /// </summary>
    public double EnemyXpDropMultiplier { get; set; } = 5.0;

    /// <summary>
    /// The chance enemies will drop loot, expressed as a percent.
    /// For example, a value of 0.33 means roughly 1 in every 3 enemies will drop loot.
    /// </summary>
    public double EnemyLootDropChance { get; set; } = 0.2;

    /// <summary>
    /// Configuration settings related to how enemies spawn in an area
    /// </summary>
    public EnemySpawnConfig EnemySpawning { get; set; } = new EnemySpawnConfig();
}
namespace GearBox.Core.Config;

public class GearBoxConfig
{
    /// <summary>
    /// When set, enemies will spawn with no AI attached.
    /// </summary>
    public bool DisableAI { get; set; } = false;

    /// <summary>
    /// Configuration settings related to how enemies spawn in an area
    /// </summary>
    public EnemySpawnConfig EnemySpawning { get; set; } = new EnemySpawnConfig();
}
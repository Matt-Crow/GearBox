using GearBox.Core.Model.Items;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.GameObjects.Enemies;

/// <summary>
/// A template used to make enemy characters
/// </summary>
public class EnemyCharacterTemplate : IFactoryProduct
{
    public EnemyCharacterTemplate(string name, Color color, List<LootOption> lootOptions)
    {
        Name = name;
        Color = color;
        LootOptions = lootOptions;
    }

    public string Name { get; init; }
    public Color Color { get; init; }
    public List<LootOption> LootOptions { get; init; }
    public string Key => Name;
}
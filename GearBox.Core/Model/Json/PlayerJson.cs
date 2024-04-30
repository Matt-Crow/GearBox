namespace GearBox.Core.Model.Json;

public readonly struct PlayerJson : IJson
{
    public PlayerJson(
        Guid id, 
        string name, 
        int level, 
        int x, 
        int y, 
        FractionJson hitPoints, 
        FractionJson energy,
        StableJson inventory,
        StableJson weapon
    )
    {
        Id = id;
        Name = name;
        Level = level;
        X = x;
        Y = y;
        HitPoints = hitPoints;
        Energy = energy;
        Inventory = inventory;
        Weapon = weapon;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public int Level { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
    public FractionJson HitPoints { get; init; }
    public FractionJson Energy { get; init; }
    public StableJson Inventory { get; init; }
    public StableJson Weapon { get; init; }
}
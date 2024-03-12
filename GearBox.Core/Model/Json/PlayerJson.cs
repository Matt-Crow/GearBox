namespace GearBox.Core.Model.Json;

public readonly struct PlayerJson : IJson
{
    public PlayerJson(Guid id, FractionJson hitPoints, FractionJson energy, InventoryJson inventory, ItemJson? weapon)
    {
        Id = id;
        HitPoints = hitPoints;
        Energy = energy;
        Inventory = inventory;
        Weapon = weapon;
    }

    public Guid Id { get; init; }
    public FractionJson HitPoints { get; init; }
    public FractionJson Energy { get; init; }
    public InventoryJson Inventory { get; init; }
    public ItemJson? Weapon { get; init; }
}
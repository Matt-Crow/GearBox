namespace GearBox.Core.Model.Json;

public readonly struct PlayerJson : IJson
{
    public PlayerJson(Guid id, FractionJson energy, InventoryJson inventory, ItemJson? weapon)
    {
        Id = id;
        Energy = energy;
        Inventory = inventory;
        Weapon = weapon;
    }

    public Guid Id { get; init; }
    public FractionJson Energy { get; init; }
    public InventoryJson Inventory { get; init; }
    public ItemJson? Weapon { get; init; }
}
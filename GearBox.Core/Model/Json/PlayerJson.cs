namespace GearBox.Core.Model.Json;

public readonly struct PlayerJson : IJson
{
    public PlayerJson(Guid id, InventoryJson inventory, ItemJson? weapon)
    {
        Id = id;
        Inventory = inventory;
        Weapon = weapon;
    }

    public Guid Id { get; init; }
    public InventoryJson Inventory { get; init; }
    public ItemJson? Weapon { get; init; }
}
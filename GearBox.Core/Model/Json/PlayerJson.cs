namespace GearBox.Core.Model.Json;

public readonly struct PlayerJson : IJson
{
    public PlayerJson(Guid id, InventoryJson inventory)
    {
        Id = id;
        Inventory = inventory;
    }

    public Guid Id { get; init; }
    public InventoryJson Inventory { get; init; }
}
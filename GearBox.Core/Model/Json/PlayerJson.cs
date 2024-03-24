namespace GearBox.Core.Model.Json;

public readonly struct PlayerJson : IJson
{
    public PlayerJson(Guid id, FractionJson energy, ItemJson? weapon)
    {
        Id = id;
        Energy = energy;
        Weapon = weapon;
    }

    public Guid Id { get; init; }
    public FractionJson Energy { get; init; }
    public ItemJson? Weapon { get; init; }
}
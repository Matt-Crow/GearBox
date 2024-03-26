namespace GearBox.Core.Model.Json;

public readonly struct PlayerJson : IJson
{
    public PlayerJson(Guid id, FractionJson energy)
    {
        Id = id;
        Energy = energy;
    }

    public Guid Id { get; init; }
    public FractionJson Energy { get; init; }
}
namespace GearBox.Core.Model.Json;

public readonly struct LootChestJson : IJson
{
    public LootChestJson(Guid id, int x, int y, List<Guid> collectedBy)
    {
        Id = id;
        X = x;
        Y = y;
        CollectedBy = collectedBy;
    }

    public Guid Id { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
    public List<Guid> CollectedBy { get; init; }
}
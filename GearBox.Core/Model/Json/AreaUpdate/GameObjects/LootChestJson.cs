namespace GearBox.Core.Model.Json.AreaUpdate.GameObjects;

public readonly struct LootChestJson : IJson
{
    public LootChestJson(int x, int y, List<Guid> collectedBy)
    {
        X = x;
        Y = y;
        CollectedBy = collectedBy;
    }

    public int X { get; init; }
    public int Y { get; init; }
    public List<Guid> CollectedBy { get; init; }
}
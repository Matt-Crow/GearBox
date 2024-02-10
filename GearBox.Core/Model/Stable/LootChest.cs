using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// A LootChest provides players with loot
/// </summary>
public class LootChest : IStableGameObject
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly HashSet<Guid> _collectedBy = new();
    private readonly List<Item> _contents;

    public LootChest(Coordinates location, params Item[] contents)
    {
        Body = new BodyBehavior()
        {
            Location = location
        };
        _contents = new List<Item>(contents);
    }

    public string Type => "lootChest";
    public IEnumerable<object?> DynamicValues => _collectedBy
        .Select(x => (object?)x) // not sure why it requires this explicit cast
        .AsEnumerable();
    public BodyBehavior Body { get; init; }

    public void CheckForCollisions(PlayerCharacter player)
    {
        if (_collectedBy.Contains(player.Inner.Id))
        {
            return;
        }

        if (Body.CollidesWith(player.Inner.Body))
        {
            _collectedBy.Add(player.Inner.Id);
            player.Inventory.Materials.AddRange(_contents);
        }
    }

    public void Update()
    {
        // do nothing
    }

    public string Serialize(JsonSerializerOptions options)
    {
        var asJson = new LootChestJson(
            _id, 
            Body.Location.XInPixels,
            Body.Location.YInPixels,
            _collectedBy.ToList()
        );
        return JsonSerializer.Serialize(asJson, options);
    }
}
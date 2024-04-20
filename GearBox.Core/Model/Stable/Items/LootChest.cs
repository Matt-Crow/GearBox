using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Dynamic.Player;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;
using System.Text.Json;

namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// A LootChest provides players with loot
/// </summary>
public class LootChest : IStableGameObject
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly HashSet<Guid> _collectedBy = [];
    private readonly Inventory _contents = new();

    public LootChest(Coordinates location, Inventory contents)
    {
        Body = new BodyBehavior()
        {
            Location = location
        };
        _contents = contents;
        Serializer = new("lootChest", Serialize);
    }

    public Serializer Serializer { get; init; }
    public IEnumerable<object?> DynamicValues => _collectedBy
        .Select(x => (object?)x) // not sure why it requires this explicit cast
        .AsEnumerable();
    public BodyBehavior Body { get; init; }

    public void CheckForCollisions(PlayerCharacter player)
    {
        if (_collectedBy.Contains(player.Id))
        {
            return;
        }

        if (Body.CollidesWith(player.Body))
        {
            _collectedBy.Add(player.Id);
            player.Inventory.Add(_contents);
        }
    }

    public void Update()
    {
        // do nothing
    }

    private string Serialize(JsonSerializerOptions options)
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
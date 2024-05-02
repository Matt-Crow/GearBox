using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;
using System.Text.Json;

namespace GearBox.Core.Model.Items;

/// <summary>
/// A LootChest provides players with loot
/// </summary>
public class LootChest : IGameObject
{
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
    public BodyBehavior Body { get; init; }
    public TerminateBehavior? Termination => null;

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

    private string Serialize(SerializationOptions options)
    {
        var asJson = new LootChestJson(
            Body.Location.XInPixels,
            Body.Location.YInPixels,
            _collectedBy.ToList()
        );
        return JsonSerializer.Serialize(asJson, options.JsonSerializerOptions);
    }
}
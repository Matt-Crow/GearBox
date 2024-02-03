
using System.Text.Json;
using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Decorator around a Character.
/// While this would make a lot of sense as a subclass of Character, since they
/// have different update types (characters are dynamic, players are stable),
/// I'll keep it in this format for now.
/// </summary>
public class PlayerCharacter : IStableGameObject
{
    private readonly Character _inner;

    public PlayerCharacter(Character inner)
    {
        _inner = inner;
    }

    public string Type => "playerCharacter";
    public IEnumerable<object?> DynamicValues => Inventory.DynamicValues;
    public Inventory Inventory { get; init; } = new();

    public void Update()
    {
        // do not update inner!
    }

    public string Serialize(JsonSerializerOptions options)
    {
        var asJson = new PlayerJson(_inner.Id, Inventory.ToJson());
        return JsonSerializer.Serialize(asJson, options);
    }
}
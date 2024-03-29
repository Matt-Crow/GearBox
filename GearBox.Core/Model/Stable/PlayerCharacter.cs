using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Decorator around a Character.
/// While this would make a lot of sense as a subclass of Character, since they
/// have different update types (characters are dynamic, players are stable),
/// I'll keep it in this format for now.
/// </summary>
public class PlayerCharacter : IStableGameObject
{
    public PlayerCharacter(Character inner)
    {
        Inner = inner;
    }

    public string Type => "playerCharacter";
    public IEnumerable<object?> DynamicValues => Inventory.DynamicValues;
    public Character Inner { get; init; }
    public Inventory Inventory { get; init; } = new();

    public void Update()
    {
        // do not update inner!
    }

    public string Serialize(JsonSerializerOptions options)
    {
        var asJson = new PlayerJson(Inner.Id, Inventory.ToJson());
        return JsonSerializer.Serialize(asJson, options);
    }
}
using System.Text.Json;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable.Items;

public class EquipmentSlot : IStableGameObject
{
    private readonly Guid _ownerId;

    public EquipmentSlot(Guid ownerId)
    {
        _ownerId = ownerId;
    }

    public Equipment? Value { get; set; }

    public string Type => "equippedWeapon"; // todo change when generics are used

    // start by outputting a boolean to distinguish between no Value and Value with no dynamic values
    public IEnumerable<object?> DynamicValues => Value == null
        ? SequenceOf(false)
        : SequenceOf(true).Append(Value.Id).Concat(Value.DynamicValues);

    public void Update()
    {
        // do nothing
    }

    public string Serialize(JsonSerializerOptions options)
    {
        var asJson = new EquipmentSlotJson(_ownerId, ValueJson());
        return JsonSerializer.Serialize(asJson, options);
    }

    private ItemJson? ValueJson()
    {
        if (Value == null)
        {
            return null;
        }
        
        var result = new ItemJson(
            Value.Id,
            Value.Type.Name,
            Value.Description,
            Value.Level,
            Value.Details,
            1 // quantity is always 1
        );
        return result;
    }

    private static IEnumerable<object?> SequenceOf(object? value) => [value];
}
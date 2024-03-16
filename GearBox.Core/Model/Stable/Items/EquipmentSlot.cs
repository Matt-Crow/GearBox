using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable.Items;

public class EquipmentSlot : IDynamic
{
    public Equipment? Value { get; set; }

    public ItemJson? ToJson()
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

    // start by outputting a boolean to distinguish between no Value and Value with no dynamic values
    public IEnumerable<object?> DynamicValues => Value == null
            ? SequenceOf(false)
            : SequenceOf(true).Concat(Value.DynamicValues);

    private static IEnumerable<object?> SequenceOf(object? value) => [value];
}
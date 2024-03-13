using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable.Items;

public class EquipmentSlot
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
            1 // quantity is always 1
        );
        return result;
    }

    public IEnumerable<object?> DynamicValues => Array.Empty<object?>();

    private IEnumerable<object?> GetDynamicValues()
    {
        // start by outputting a boolean to distinguish between no Value and Value with no dynamic values
        var result = Value == null
            ? SequenceOf(false)
            : SequenceOf(true).Concat(Value.DynamicValues);
        return result;
    }

    private static IEnumerable<object?> SequenceOf(object? value)
    {
        return new List<object?>()
        {
            value
        };
    }
}
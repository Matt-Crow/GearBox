using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Items;

public class EquipmentSlot<T>
where T : IEquipmentStats
{
    public Equipment<T>? Value { get; set; }

    private ItemJson? ValueJson()
    {
        if (Value == null)
        {
            return null;
        }
        
        var result = new ItemJson(
            Value.Id,
            Value.Name,
            Value.Grade.Name,
            Value.Grade.Order,
            Value.Description,
            Value.Level,
            Value.Details,
            1 // quantity is always 1
        );
        return result;
    }

    public ItemJson? ToJson() => ValueJson();
}
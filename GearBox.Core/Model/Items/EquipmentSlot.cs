using System.Text.Json;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.GameObjects.ChangeTracking;

namespace GearBox.Core.Model.Items;

public class EquipmentSlot<T> : IDynamic
where T : Equipment
{
    private readonly ChangeTracker _changeTracker;

    public EquipmentSlot(string type)
    {
        _changeTracker = new(this);
    }

    public string Serialize(SerializationOptions options) => JsonSerializer.Serialize(ValueJson(), options.JsonSerializerOptions);

    public T? Value { get; set; }

    public IEnumerable<object?> DynamicValues => Value == null
        ? [false]
        : [true, Value.Id];

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

    public void Update() => _changeTracker.Update();
    public StableJson ToJson() => _changeTracker.ToJson();
}
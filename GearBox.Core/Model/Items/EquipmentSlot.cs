using System.Text.Json;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.GameObjects.ChangeTracking;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Items;

public class EquipmentSlot<T> : IDynamic
where T : Equipment
{
    private readonly ChangeTracker _changeTracker;
    private bool _updatedLastFrame = true;

    public EquipmentSlot(string type)
    {
        Serializer = new Serializer(
            type,
            options => JsonSerializer.Serialize(ValueJson(), options.JsonSerializerOptions)
        );
        _changeTracker = new(this);
    }

    public T? Value { get; set; }

    public Serializer Serializer { get; init; }

    // start by outputting a boolean to distinguish between no Value and Value with no dynamic values
    public IEnumerable<object?> DynamicValues => Value == null
        ? ListExtensions.Of<object?>(false)
        : ListExtensions.Of<object?>(true).Append(Value.Id).Concat(Value.DynamicValues);
    
    public StableJson ToJson()
    {
        var result = _updatedLastFrame // _changeTracker.HasChanged is cleared before it gets here
            ? StableJson.Changed(Serializer.Serialize().Content)
            : StableJson.NoChanges();
        return result;
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

    public void Update()
    {
        _updatedLastFrame = _changeTracker.HasChanged;
        _changeTracker.Update();
    }
}
using System.Text.Json;
using GearBox.Core.Model.Json;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Stable.Items;

public class EquipmentSlot<T> : IDynamic
where T : Equipment
{
    private readonly Guid _ownerId;
    private readonly ChangeTracker _changeTracker;
    private bool _updatedLastFrame = true;

    public EquipmentSlot(Guid ownerId, string type)
    {
        _ownerId = ownerId;
        Serializer = new Serializer(
            type,
            options => JsonSerializer.Serialize(new EquipmentSlotJson(_ownerId, ValueJson()), options.JsonSerializerOptions)
        );
        _changeTracker = new(this);
    }

    public T? Value { get; set; }

    public Serializer Serializer { get; init; }

    // start by outputting a boolean to distinguish between no Value and Value with no dynamic values
    public IEnumerable<object?> DynamicValues => Value == null
        ? ListExtensions.Of<object?>(false)
        : ListExtensions.Of<object?>(true).Append(Value.Id).Concat(Value.DynamicValues);
    
    public StableJson ToJson(bool isWorldInit)
    {
        var result = _updatedLastFrame || isWorldInit // _changeTracker.HasChanged is cleared before it gets here
            ? StableJson.Changed(Serializer.Serialize(isWorldInit).Content)
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
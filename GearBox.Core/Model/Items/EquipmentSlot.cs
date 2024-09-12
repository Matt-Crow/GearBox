using GearBox.Core.Model.Json;
using GearBox.Core.Model.GameObjects.ChangeTracking;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Items;

public class EquipmentSlot<T> : IMightChange<ItemJson?>
where T : IEquipmentStats
{
    private readonly ChangeTracker<ItemJson?> _changeTracker;

    public EquipmentSlot()
    {
        _changeTracker = new(this);
    }

    public Equipment<T>? Value { get; set; }

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

    public void Update() => _changeTracker.Update();
    public MaybeChangeJson<ItemJson?> GetChanges() => _changeTracker.ToJson();
    public ItemJson? ToJson() => ValueJson();
}
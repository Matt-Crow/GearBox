using GearBox.Core.Model.Json;
using GearBox.Core.Model.GameObjects.ChangeTracking;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Items;

public class EquipmentSlot<T, U> : IMightChange<ItemJson?>
where T : Equipment<U>
where U : IEquipmentStats // todo not 2 generics
{
    private readonly ChangeTracker<ItemJson?> _changeTracker;

    public EquipmentSlot(string type)
    {
        _changeTracker = new(this);
    }

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
    public MaybeChangeJson<ItemJson?> GetChanges() => _changeTracker.ToJson();
    public ItemJson? ToJson() => ValueJson();
}
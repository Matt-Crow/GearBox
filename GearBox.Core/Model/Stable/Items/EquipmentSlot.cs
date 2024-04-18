using System.Text.Json;
using GearBox.Core.Model.Json;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Stable.Items;

public class EquipmentSlot<T> : IStableGameObject
where T : Equipment
{
    private readonly Guid _ownerId;

    public EquipmentSlot(Guid ownerId)
    {
        _ownerId = ownerId;
        Serializer = new Serializer(
            "equippedWeapon", // todo change when generics are used
            options => JsonSerializer.Serialize(new EquipmentSlotJson(_ownerId, ValueJson()), options)
        );
    }

    public T? Value { get; set; }

    public Serializer Serializer { get; init; }

    // start by outputting a boolean to distinguish between no Value and Value with no dynamic values
    public IEnumerable<object?> DynamicValues => Value == null
        ? ListExtensions.Of<object?>(false)
        : ListExtensions.Of<object?>(true).Append(Value.Id).Concat(Value.DynamicValues);

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
        // do nothing
    }
}
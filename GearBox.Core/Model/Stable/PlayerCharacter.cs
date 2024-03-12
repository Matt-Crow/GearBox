using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Decorator around a Character.
/// While this would make a lot of sense as a subclass of Character, since they
/// have different update types (characters are dynamic, players are stable),
/// I'll keep it in this format for now.
/// </summary>
public class PlayerCharacter : IStableGameObject
{
    private int _frameCount = 0;

    public PlayerCharacter(Character inner)
    {
        Inner = inner;
    }

    public PlayerCharacter() : this(new())
    {

    }

    public string Type => "playerCharacter";
    public IEnumerable<object?> DynamicValues => Inventory.DynamicValues
        .Concat(HitPoints.DynamicValues)
        .Concat(Energy.DynamicValues)
        .Concat(Weapon.DynamicValues);
    
    public Character Inner { get; init; }
    public Fraction HitPoints { get; init; } = new(50, 100); // test value for now
    public Fraction Energy { get; init; } = new(100, 200); // test value for now
    public Inventory Inventory { get; init; } = new();
    public EquipmentSlot Weapon { get; init; } = new();
    
    public void Equip(Equipment equipment)
    {
        if (!Inventory.Equipment.Contains(equipment))
        {
            throw new ArgumentException(nameof(equipment));
        }

        var slot = equipment.GetSlot(this);
        
        // check if something is already in the slot
        if (slot.Value != null)
        {
            Inventory.Add(slot.Value);
        }

        slot.Value = equipment;
        Inventory.Remove(equipment);
    }

    public void EquipById(Guid id)
    {
        var equipment = Inventory.Equipment.GetItemById(id) as Equipment;
        if (equipment != null)
        {
            Equip(equipment);
        }
    }

    public void Update()
    {
        // do not update inner!

        // restore 5% HP & energy per second
        _frameCount++;
        if (_frameCount >= Time.FRAMES_PER_SECOND)
        {
            HitPoints.RestorePercent(0.05);
            Energy.RestorePercent(0.05);
            _frameCount = 0;
        }
    }

    public string Serialize(JsonSerializerOptions options)
    {
        var asJson = new PlayerJson(
            Inner.Id, 
            HitPoints.ToJson(),
            Energy.ToJson(),
            Inventory.ToJson(), 
            Weapon.ToJson()
        );
        return JsonSerializer.Serialize(asJson, options);
    }
}
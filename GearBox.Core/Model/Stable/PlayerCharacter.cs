using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;
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
    private static readonly Speed BASE_SPEED = Speed.FromTilesPerSecond(3);
    private int _damageTaken = 0; // track damage taken instead of remaining HP to avoid issues when swapping armor
    private int _energyExpended = 0; // track energy expended instead of remaining energy to avoid issues when swapping equipment
    private int _frameCount = 0;

    public PlayerCharacter() 
    {
        Inner = new(Velocity.FromPolar(BASE_SPEED, Direction.DOWN));
        UpdateStats();
    }

    public string Type => "playerCharacter";
    public IEnumerable<object?> DynamicValues => Inventory.DynamicValues
        .Append(_damageTaken)
        .Append(_energyExpended)
        .Concat(Stats.DynamicValues)
        .Concat(Weapon.DynamicValues);
    
    public Character Inner { get; init; } = new();
    public PlayerStats Stats { get; init; } = new();
    private int MaxHitPoints => Stats.MaxHitPoints.Value;
    private int MaxEnergy => Stats.MaxEnergy.Value;
    public Inventory Inventory { get; init; } = new();
    public EquipmentSlot Weapon { get; init; } = new();

    private void UpdateStats()
    {
        var allStatBoosts = new List<PlayerStatBoosts>();
        if (Weapon.Value != null)
        {
            allStatBoosts.Add(Weapon.Value.StatBoosts);
        }
        Stats.SetStatBoosts(allStatBoosts);

        // update movement speed
        var multiplier = 1.0+Stats.Speed.Value;
        Inner.SetSpeed(Speed.FromPixelsPerFrame(BASE_SPEED.InPixelsPerFrame * multiplier));
    }
    
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

        UpdateStats();
    }

    public void EquipById(Guid id)
    {
        var equipment = Inventory.Equipment.GetItemById(id) as Equipment;
        if (equipment != null)
        {
            Equip(equipment);
        }
    }

    public void HealPercent(double percent)
    {
        _damageTaken -= (int)(MaxHitPoints*percent);
        if (_damageTaken < 0)
        {
            _damageTaken = 0;
        }
    }

    public void RechargePercent(double percent)
    {
        _energyExpended -= (int)(MaxEnergy*percent);
        if (_energyExpended < 0)
        {
            _energyExpended = 0;
        }
    }

    public void Update()
    {
        // do not update inner!

        // restore 5% HP & energy per second
        _frameCount++;
        if (_frameCount >= Time.FRAMES_PER_SECOND)
        {
            HealPercent(0.05);
            RechargePercent(0.05);
            _frameCount = 0;
        }
    }

    public string Serialize(JsonSerializerOptions options)
    {
        var asJson = new PlayerJson(
            Inner.Id, 
            new FractionJson(MaxHitPoints - _damageTaken, MaxHitPoints),
            new FractionJson(MaxEnergy - _energyExpended, MaxEnergy),
            Inventory.ToJson(), 
            Weapon.ToJson()
        );
        return JsonSerializer.Serialize(asJson, options);
    }
}
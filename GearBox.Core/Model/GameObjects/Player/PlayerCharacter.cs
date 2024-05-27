using GearBox.Core.Model.Json;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;
using System.Text.Json;

namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerCharacter : Character
{
    private int _energyExpended = 0; // track energy expended instead of remaining energy to avoid issues when swapping equipment
    private int _frameCount = 0; // used for regeneration

    public PlayerCharacter(string name, int level) : base(name, level)
    {
        Inventory = new();
        Weapon = new("equippedWeapon");
        UpdateStats();
    }

    protected override string Type => "playerCharacter";
    private int MaxEnergy { get; set; }
    public PlayerStats Stats { get; init; } = new();
    public Inventory Inventory { get; init; }
    public EquipmentSlot<Weapon> Weapon { get; init; }

    public override void UpdateStats()
    {
        var boosts = PlayerStatBoosts.Empty();
        boosts = boosts.Combine(Weapon.Value?.StatBoosts);
        Stats.SetStatBoosts(boosts);

        MaxEnergy = GetMaxEnergyByLevel(Level);

        DamageModifier = Stats.Offense.Value;

        // update movement speed
        var multiplier = 1.0+Stats.Speed.Value;
        SetSpeed(Speed.FromPixelsPerFrame(BASE_SPEED.InPixelsPerFrame * multiplier));
        
        base.UpdateStats();
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (!Inventory.Weapons.Contains(weapon))
        {
            throw new ArgumentException(nameof(weapon));
        }

        var slot = Weapon;
        
        // check if something is already in the slot
        if (slot.Value != null)
        {
            Inventory.Weapons.Add(slot.Value);
        }

        slot.Value = weapon;
        Inventory.Weapons.Remove(weapon);
        BasicAttack.Range = weapon.AttackRange;

        UpdateStats();
    }

    public void EquipWeaponById(Guid id)
    {
        var equipment = Inventory.Weapons.GetItemById(id);
        if (equipment != null)
        {
            EquipWeapon(equipment);
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

    public override void Update()
    {
        base.Update();

        Inventory.Update();
        Weapon.Update();

        // restore 5% HP & energy per second
        _frameCount++;
        if (_frameCount >= Duration.FromSeconds(5).InFrames)
        {
            HealPercent(0.05);
            RechargePercent(0.05);
            _frameCount = 0;
        }
    }

    protected override string Serialize(SerializationOptions options)
    {
        var asJson = new PlayerJson(
            Id, 
            Name,
            Level,
            Coordinates.XInPixels,
            Coordinates.YInPixels,
            new FractionJson(MaxHitPoints - DamageTaken, MaxHitPoints),
            new FractionJson(MaxEnergy - _energyExpended, MaxEnergy)
        );
        return JsonSerializer.Serialize(asJson, options.JsonSerializerOptions);
    }

    protected override int GetMaxHitPointsByLevel(int level)
    {
        var multiplier = 1.0 + Stats.MaxHitPoints.Value;
        var result = base.GetMaxHitPointsByLevel(level) * multiplier;
        return (int)result; 
    }

    private int GetMaxEnergyByLevel(int level)
    {
        var multiplier = 1.0 + Stats.MaxEnergy.Value;
        var result = (50 + 5*level) * multiplier;
        return (int)result;
    }
}
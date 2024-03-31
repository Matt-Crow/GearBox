using GearBox.Core.Model.Json;
using GearBox.Core.Model.Stable.Items;
using GearBox.Core.Model.Units;
using System.Text.Json;

namespace GearBox.Core.Model.Dynamic.Player;

public class PlayerCharacter : Character
{
    private int _energyExpended = 0; // track energy expended instead of remaining energy to avoid issues when swapping equipment
    private int _frameCount = 0; // used for regeneration
    private int _basicAttackCooldownInFrames = 0;

    public PlayerCharacter() : base()
    {
        Inventory = new(Id);
        Weapon = new(Id);
        UpdateStats();
    }

    protected override string Type => "playerCharacter";
    private int MaxEnergy { get; set; }
    public PlayerStats Stats { get; init; } = new();
    public Inventory Inventory { get; init; }
    public EquipmentSlot Weapon { get; init; }

    public override void UpdateStats()
    {
        var boosts = new PlayerStatBoosts();
        boosts = boosts.Combine(Weapon.Value?.StatBoosts);
        Stats.SetStatBoosts(boosts);

        MaxEnergy = GetMaxEnergyByLevel(Level);

        // update movement speed
        var multiplier = 1.0+Stats.Speed.Value;
        SetSpeed(Speed.FromPixelsPerFrame(BASE_SPEED.InPixelsPerFrame * multiplier));
        
        base.UpdateStats();
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

    public void RechargePercent(double percent)
    {
        _energyExpended -= (int)(MaxEnergy*percent);
        if (_energyExpended < 0)
        {
            _energyExpended = 0;
        }
    }

    public void UseBasicAttack(World inWorld, Direction inDirection)
    {
        if (_basicAttackCooldownInFrames != 0)
        {
            return;
        }
        
        var weapon = Weapon.Value as Weapon;
        var range = weapon?.AttackRange.Range ?? AttackRange.MELEE.Range;
        var damage = DamagePerHit * (1.0 + Stats.Offense.Value);
        var attack = new Attack(this, (int)damage);
        var projectile = new Projectile(
            Coordinates, 
            Velocity.FromPolar(Speed.FromTilesPerSecond(range.InTiles), inDirection),
            range,
            attack
        );
        inWorld.DynamicContent.AddDynamicObject(projectile);

        _basicAttackCooldownInFrames = Duration.FromSeconds(0.5).InFrames;
    }

    public override void TakeDamage(int damage)
    {
        var reducedDamage = damage * (1.0 - Stats.Defense.Value);
        base.TakeDamage((int)reducedDamage);
    }

    public override void Update()
    {
        base.Update();

        // restore 5% HP & energy per second
        _frameCount++;
        if (_frameCount >= Duration.FromSeconds(5).InFrames)
        {
            HealPercent(0.05);
            RechargePercent(0.05);
            _frameCount = 0;
        }

        _basicAttackCooldownInFrames--;
        if (_basicAttackCooldownInFrames < 0)
        {
            _basicAttackCooldownInFrames = 0;
        }
    }

    protected override string Serialize(JsonSerializerOptions options)
    {
        // serialize neither inventory nor weapon - those are handled elsewhere
        var asJson = new PlayerJson(
            Id, 
            Coordinates.XInPixels,
            Coordinates.YInPixels,
            new FractionJson(MaxHitPoints - DamageTaken, MaxHitPoints),
            new FractionJson(MaxEnergy - _energyExpended, MaxEnergy)
        );
        return JsonSerializer.Serialize(asJson, options);
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
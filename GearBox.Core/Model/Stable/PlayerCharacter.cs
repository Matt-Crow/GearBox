using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Stable.Items;
using GearBox.Core.Model.Units;
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Decorator around a Character.
/// While this would make a lot of sense as a subclass of Character, since they
/// have different update types (characters are dynamic, players are stable),
/// I'll keep it in this format for now.
/// </summary>
public class PlayerCharacter : Character
{
    /// <summary>
    /// The maximum level a player can attain
    /// </summary>
    public static readonly int MAX_LEVEL = 20;
    private static readonly Duration REGEN_COOLDOWN = Duration.FromSeconds(5);
    private static readonly Speed BASE_SPEED = Speed.FromTilesPerSecond(3);
    private readonly int _level = MAX_LEVEL; // in the future, this will read from a repository
    
    /// <summary>
    /// While it seems a bit strange to keep damage per hit on the player instead of their weapon,
    /// this prevents situations where all weapons would require damage 
    /// and allows players to attack without weapons
    /// </summary>
    private int _damagePerHit;
    private int _energyExpended = 0; // track energy expended instead of remaining energy to avoid issues when swapping equipment
    private int _frameCount = 0; // used for regeneration
    private int _basicAttackCooldownInFrames = 0;

    public PlayerCharacter() : base(Velocity.FromPolar(BASE_SPEED, Direction.DOWN))
    {
        Inventory = new(Id);
        Weapon = new(Id);
        UpdateStats();
    }

    protected override string Type => "playerCharacter";
    public PlayerStats Stats { get; init; } = new();
    private int MaxEnergy => Stats.MaxEnergy.Value;
    public Inventory Inventory { get; init; }
    public EquipmentSlot Weapon { get; init; }

    private void UpdateStats()
    {
        // move to leveling method later
        _damagePerHit = GetDamagePerHitByLevel(_level);

        // scale HP & energy with level
        var boosts = new PlayerStatBoosts(new()
        {
            {PlayerStatType.MAX_HIT_POINTS, _level * 20},
            {PlayerStatType.MAX_ENERGY, _level * 20}
        });
        boosts = boosts.Combine(Weapon.Value?.StatBoosts);
        Stats.SetStatBoosts(boosts);
        MaxHitPoints = Stats.MaxHitPoints.Value;

        // update movement speed
        var multiplier = 1.0+Stats.Speed.Value;
        SetSpeed(Speed.FromPixelsPerFrame(BASE_SPEED.InPixelsPerFrame * multiplier));
    }

    private static int GetDamagePerHitByLevel(int level)
    {
        var maxDamage = 500;
        var percentToMaxLevel = ((double)level) / MAX_LEVEL;
        var result = (int)(maxDamage * percentToMaxLevel);
        return result;
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
        var damage = _damagePerHit * (1.0 + Stats.Offense.Value);
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
        if (_frameCount >= REGEN_COOLDOWN.InFrames)
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
}
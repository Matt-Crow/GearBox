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
public class PlayerCharacter : IDynamicGameObject
{
    /// <summary>
    /// The maximum level a player can attain
    /// </summary>
    public static readonly int MAX_LEVEL = 20;
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

    public PlayerCharacter() 
    {
        Inner = new(Velocity.FromPolar(BASE_SPEED, Direction.DOWN));
        Inventory = new(Inner.Id);
        Weapon = new(Inner.Id);
        UpdateStats();
    }

    public string Type => "playerCharacter";
    public Character Inner { get; init; } = new();
    public BodyBehavior Body => Inner.Body;
    public bool IsTerminated => Inner.IsTerminated;
    public PlayerStats Stats { get; init; } = new();
    private int MaxEnergy => Stats.MaxEnergy.Value;
    public Inventory Inventory { get; init; }
    public EquipmentSlot Weapon { get; init; }

    private void UpdateStats()
    {
        // move to leveling method later
        _damagePerHit = GetDamagePerHitByLevel(_level);

        var boosts = new PlayerStatBoosts();
        boosts = boosts.Combine(Weapon.Value?.StatBoosts);
        Stats.SetStatBoosts(boosts);
        Inner.MaxHitPoints = Stats.MaxHitPoints.Value;

        // update movement speed
        var multiplier = 1.0+Stats.Speed.Value;
        Inner.SetSpeed(Speed.FromPixelsPerFrame(BASE_SPEED.InPixelsPerFrame * multiplier));
    }

    private static int GetDamagePerHitByLevel(int level)
    {
        var maxDamage = 1000;
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
        var attack = new Attack(Inner);
        var projectile = new Projectile(
            Inner.Coordinates, 
            Velocity.FromPolar(Speed.FromTilesPerSecond(range.InTiles), inDirection),
            range,
            attack
        );
        inWorld.DynamicContent.AddDynamicObject(projectile);

        _basicAttackCooldownInFrames = Time.FRAMES_PER_SECOND / 2; // .5 second cooldown
    }

    public void Update()
    {
        // do not call Inner.Update!

        // restore 5% HP & energy per second
        _frameCount++;
        if (_frameCount >= Time.FRAMES_PER_SECOND)
        {
            Inner.HealPercent(0.05);
            RechargePercent(0.05);
            _frameCount = 0;
        }

        _basicAttackCooldownInFrames--;
        if (_basicAttackCooldownInFrames < 0)
        {
            _basicAttackCooldownInFrames = 0;
        }
    }

    public string Serialize(JsonSerializerOptions options)
    {
        // serialize neither inventory nor weapon - those are handled elsewhere
        var asJson = new PlayerJson(
            Inner.Id, 
            new FractionJson(MaxEnergy - _energyExpended, MaxEnergy)
        );
        return JsonSerializer.Serialize(asJson, options);
    }
}
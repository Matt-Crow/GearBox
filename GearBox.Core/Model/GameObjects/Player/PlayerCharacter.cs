using GearBox.Core.Model.Json;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;
using System.Text.Json;

namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerCharacter : Character
{
    private int _energyExpended = 0; // track energy expended instead of remaining energy to avoid issues when swapping equipment
    private int _frameCount = 0; // used for regeneration
    private int _xp = 0; // experience points
    private int _xpToNextLevel;

    public PlayerCharacter(string name, int xp=0) : base(name, GetLevelByXp(xp))
    {
        Inventory = new();
        WeaponSlot = new("equippedWeapon");
        ArmorSlot = new("equippedArmor");
        _xp = xp;
        _xpToNextLevel = GetXpByLevel(Level + 1);
        UpdateStats();
    }

    protected override string Type => "playerCharacter";
    private int MaxEnergy { get; set; }
    public PlayerStats Stats { get; init; } = new();
    public Inventory Inventory { get; init; }
    public EquipmentSlot<Weapon> WeaponSlot { get; init; }
    public EquipmentSlot<Armor> ArmorSlot { get; init; }

    public override void UpdateStats()
    {
        var boosts = PlayerStatBoosts.Empty()
            .Combine(WeaponSlot.Value?.StatBoosts)
            .Combine(ArmorSlot.Value?.StatBoosts);
        Stats.SetStatBoosts(boosts);

        MaxEnergy = GetMaxEnergyByLevel(Level);

        DamageModifier = Stats.Offense.Value;

        // update movement speed
        var multiplier = 1.0+Stats.Speed.Value;
        SetSpeed(Speed.FromPixelsPerFrame(BASE_SPEED.InPixelsPerFrame * multiplier));
        
        base.UpdateStats();
    }

    public void EquipWeaponById(Guid id)
    {
        var weapon = Inventory.Weapons.GetItemById(id);
        if (weapon == null || weapon.Level > Level)
        {
            return;
        }

        Inventory.Weapons.Add(WeaponSlot.Value); // put old weapon back in inventory

        WeaponSlot.Value = weapon;
        Inventory.Weapons.Remove(weapon);
        BasicAttack.Range = weapon.AttackRange;

        UpdateStats();
    }

    public void EquipArmorById(Guid id)
    {
        var armor = Inventory.Armors.GetItemById(id);
        if (armor == null || armor.Level > Level)
        {
            return;
        }

        Inventory.Armors.Add(ArmorSlot.Value); // put old armor back in inventory

        ArmorSlot.Value = armor;
        Inventory.Armors.Remove(armor);
        ArmorClass = armor.ArmorClass;

        UpdateStats();
    }

    public void GainXp(int xp)
    {
        // untested
        _xp += xp;
        while (_xp >= _xpToNextLevel)
        {
            SetLevel(Level + 1);
            UpdateStats();
            _xpToNextLevel = GetXpByLevel(Level + 1);
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
        WeaponSlot.Update();

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
        var result = base.GetMaxHitPointsByLevel(level) * multiplier * 10;
        return (int)result; 
    }

    private int GetMaxEnergyByLevel(int level)
    {
        var multiplier = 1.0 + Stats.MaxEnergy.Value;
        var result = (50 + 5*level) * multiplier;
        return (int)result;
    }

    private static int GetLevelByXp(int xp)
    {
        // don't use mathematical inverse here, in case I change GetXpByLevel
        for (var lv = MAX_LEVEL; lv > 0; lv--)
        {
            if (xp >= GetXpByLevel(lv))
            {
                return lv;
            }
        }
        return 1;
    }

    private static int GetXpByLevel(int level)
    {
        /*
            lv | xp
            1  | 0
            2  | 20
            3  | 60
            19 | 5.2m
            20 | 10m
        */
        var result = 20 * (Math.Pow(2, level - 1) - 1);
        return (int)result;
    }
}
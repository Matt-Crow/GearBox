using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;
using System.Text.Json;
using GearBox.Core.Model.Json.AreaUpdate;
using GearBox.Core.Model.Json.AreaInit;

namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerCharacter : Character
{
    private int _frameCount = 0; // used for regeneration
    private bool _hasAreaChanged = false;
    private readonly PlayerStatSummary _statSummary;

    public PlayerCharacter(string name, int xp=0) : base(name, GetLevelByXp(xp), Color.BLUE)
    {
        Xp = xp;
        XpToNextLevel = GetXpByLevel(Level + 1);
        _statSummary = new PlayerStatSummary(this);
        UpdateStats();
    }

    public event EventHandler<AreaChangedEventArgs>? AreaChanged;

    protected override string Type => "playerCharacter";
    public int Xp { get; private set; } // experience points
    public int XpToNextLevel { get; private set; }
    public int MaxEnergy { get; private set; }
    public int EnergyExpended { get; private set; } = 0; // track energy expended instead of remaining energy to avoid issues when swapping equipment
    public int EnergyRemaining => MaxEnergy - EnergyExpended;
    public PlayerStats Stats { get; init; } = new();
    public Inventory Inventory { get; init; } = new();
    public EquipmentSlot<WeaponStats> WeaponSlot { get; init; } = new();
    public EquipmentSlot<ArmorStats> ArmorSlot { get; init; } = new();

    public override void SetArea(IArea? newArea)
    {
        AreaChanged?.Invoke(this, new AreaChangedEventArgs(this, CurrentArea, newArea));
        _hasAreaChanged = true;
        base.SetArea(newArea);
    }

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
        BasicAttack.Range = weapon.Inner.AttackRange;

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
        ArmorClass = armor.Inner.ArmorClass;

        UpdateStats();
    }

    public void GainXp(int xp)
    {
        // untested
        Xp += xp;
        while (Xp >= XpToNextLevel)
        {
            SetLevel(Level + 1);
            UpdateStats();
            XpToNextLevel = GetXpByLevel(Level + 1);
        }
    }

    public void RechargePercent(double percent)
    {
        EnergyExpended -= (int)(MaxEnergy*percent);
        if (EnergyExpended < 0)
        {
            EnergyExpended = 0;
        }
    }

    public override void Update()
    {
        base.Update();

        Inventory.Update();
        WeaponSlot.Update();
        ArmorSlot.Update();

        // restore 5% HP & energy per second
        _frameCount++;
        if (_frameCount >= Duration.FromSeconds(5).InFrames)
        {
            HealPercent(0.05);
            RechargePercent(0.05);
            _frameCount = 0;
        }

        _statSummary.Update();
    }

    public ChangesJson GetChanges()
    {
        var result = new ChangesJson(
            _hasAreaChanged && CurrentArea != null
                ? MaybeChangeJson<MapJson>.Changed(CurrentArea.GetMapJson()) 
                : MaybeChangeJson<MapJson>.NoChanges(),
            _hasAreaChanged && CurrentArea != null
                ? MaybeChangeJson<List<ShopInitJson>>.Changed(CurrentArea.GetShopInitJsons())   
                : MaybeChangeJson<List<ShopInitJson>>.NoChanges(),
            Inventory.GetChanges(),
            WeaponSlot.GetChanges(),
            ArmorSlot.GetChanges(), 
            _statSummary.GetChanges()
        );
        _hasAreaChanged = false; // hacky for now
        return result;
    }

    protected override string Serialize(SerializationOptions options)
    {
        var asJson = new PlayerJson(
            Id, 
            Name,
            Level,
            Color.ToJson(),
            Coordinates.XInPixels,
            Coordinates.YInPixels,
            new FractionJson(MaxHitPoints - DamageTaken, MaxHitPoints),
            new FractionJson(MaxEnergy - EnergyExpended, MaxEnergy)
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
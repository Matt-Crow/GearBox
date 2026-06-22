using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;
using System.Text.Json;
using GearBox.Core.Model.Items.Shops;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Json.AreaUpdate.GameObjects;
using GearBox.Core.Utils;
using GearBox.Core.Model.Abilities.Passives;

namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerCharacter : Character
{
    private int _frameCount = 0; // used for regeneration
    private readonly List<IActiveAbility> _actives = [];
    private readonly List<IPassiveAbility> _passives = [];


    public PlayerCharacter(string name, int xp = 0, Guid? id = null) : base(name, GetLevelByXp(xp), Color.BLUE, id)
    {
        Xp = xp;
        XpToNextLevel = GetXpByLevel(Level + 1);
        StatSummary = new PlayerStatSummary(this);
        UpdateStats();
    }
    

    public EventEmitter<AreaChangedEvent> EventAreaChanged { get; } = new();

    protected override string Type => "playerCharacter";
    public int Xp { get; private set; } // experience points
    public int XpToNextLevel { get; private set; }
    public int MaxEnergy { get; private set; }
    public int EnergyExpended { get; private set; } = 0; // track energy expended instead of remaining energy to avoid issues when swapping parts
    public int EnergyRemaining => MaxEnergy - EnergyExpended;
    public PlayerStats Stats { get; init; } = new();
    public PlayerStatSummary StatSummary { get; init; }
    public IEnumerable<IActiveAbility> Actives => _actives;
    public IEnumerable<IPassiveAbility> Passives => _passives;
    public Inventory Inventory { get; init; } = new();

    /// <summary>
    /// Slots where parts can be installed
    /// </summary>
    public List<PartSlot> PartSlots { get; init; } = PartSlotType.ALL.Select(slotType => new PartSlot(slotType)).ToList();
    
    /// <summary>
    /// The shop the player currently has open
    /// </summary>
    public ItemShop? OpenShop { get; private set; }

    public override void SetArea(IArea? newArea)
    {
        SetOpenShop(null);
        EventAreaChanged.ProcessEvent(new AreaChangedEvent(this, CurrentArea, newArea), e =>
        {
            base.SetArea(e.NewArea);
        });
    }

    public void UseActive(int number, Direction inDirection)
    {
        var i = number - 1;
        if (0 <= i && i < _actives.Count)
        {
            _actives[i].Use(inDirection);
        }
    }

    public void SetOpenShop(ItemShop? shop)
    {
        OpenShop = shop;
    }

    public override void UpdateStats()
    {
        var boosts = PlayerStatBoosts.Empty();
        foreach (var slot in PartSlots)
        {
            boosts = boosts.Combine(slot.Part?.StatBoosts);
        }
        Stats.SetStatBoosts(boosts);

        MaxEnergy = GetMaxEnergyByLevel(Level);

        DamageModifier = Stats.Offense.Value;

        // update movement speed
        var multiplier = 1.0+Stats.Speed.Value;
        SetSpeed(Speed.FromPixelsPerFrame(BASE_SPEED.InPixelsPerFrame * multiplier));
        
        // unregister old passives
        foreach (var passive in _passives)
        {
            passive.SetUser(null);
        }

        _actives.Clear();
        _passives.Clear();
        foreach (var part in PartSlots.Select(slot => slot.Part))
        {
            if (part != null)
            {
                _actives.AddRange(part.Actives);
                _passives.AddRange(part.Passives);
            }
        }
        foreach (var active in _actives)
        {
            active.User = this;
        }
        foreach (var passive in _passives)
        {
            passive.SetUser(this);
        }

        base.UpdateStats();
    }

    /// <summary>
    /// Installs the part with the given ID if it is in the inventory and is installable.
    /// </summary>
    public void InstallById(Guid id)
    {
        var maybeItem = Inventory.GetBySpecifier(ItemSpecifier.ById(id));
        if (maybeItem == null)
        {
            return;
        }

        maybeItem.Match(
            material => {},
            InstallFromInventory
        );
    }

    private void InstallFromInventory(Part part)
    {
        if (part.Level > Level)
        {
            return;
        }

        // determine where the part goes
        var tab = Inventory.GetTab(part);
        var slot = GetSlotFor(part);
        
        // swap the old and new ones from the slot to the inventory
        tab.Add(slot.Part);
        slot.Part = part;
        tab.Remove(part);

        UpdateStats();
    }

    /// <summary>
    /// Returns the slot the given part can be slotted into.
    /// </summary>
    public PartSlot GetSlotFor(Part part)
    {
        return GetSlotFor(part.SlotType);
    }

    /// <summary>
    /// Returns the slot the given type of part can be slotted into.
    /// </summary>
    public PartSlot GetSlotFor(PartSlotType slotType)
    {
        var slot = PartSlots
            .FirstOrDefault(slot => slot.SlotType == slotType) 
            ?? throw new Exception($"PlayerCharacter is missing part slot for type {slotType.Name}");
        return slot;
    }

    public void GainXp(int xp)
    {
        Xp += xp;
        while (Xp >= XpToNextLevel)
        {
            SetLevel(Level + 1);
            UpdateStats();
            XpToNextLevel = GetXpByLevel(Level + 1);
        }
    }

    public void LoseEnergy(int energy)
    {
        EnergyExpended += energy;
        if (EnergyExpended > MaxEnergy)
        {
            EnergyExpended = MaxEnergy;
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

        // restore 5% HP & energy per second
        _frameCount++;
        if (_frameCount >= Duration.FromSeconds(1).InFrames)
        {
            HealPercent(0.05);
            RechargePercent(0.05);
            _frameCount = 0;
        }

        foreach (var active in _actives)
        {
            active.Update();
        }

        foreach (var passive in _passives)
        {
            passive.Update();
        }
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
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Areas;

public class AreaBuilder
{
    private readonly IGameBuilder _gameBuilder;
    private Map? _map;
    private readonly LootTable _loot = new();
    private readonly List<Func<Character>> _enemies = [];
    private readonly List<IExit> _exits = [];

    public AreaBuilder(string name, IGameBuilder gameBuilder)
    {
        Name = name;
        _gameBuilder = gameBuilder;
    }

    /// <summary>
    /// The name of the area this is building
    /// </summary>
    public string Name { get; init; }

    public AreaBuilder AddLoot(Action<LootTable> withLoot)
    {
        withLoot(_loot);
        return this;
    }

    public AreaBuilder DefineMaterial(Material material)
    {
        _gameBuilder.WithItemType(material.Type);
        _loot.AddMaterial(material); // all materials are loot for now
        return this;
    }

    public AreaBuilder DefineWeapon(EquipmentBuilder<Weapon> builder, bool isLoot)
    {
        _gameBuilder.WithItemType(builder.ItemType);
        if (isLoot)
        {
            _loot.AddWeapon(builder.Build(1)); // todo use area level
        }
        return this;
    }

    public AreaBuilder DefineArmor(EquipmentBuilder<Armor> builder, bool isLoot)
    {
        _gameBuilder.WithItemType(builder.ItemType);
        if (isLoot)
        {
            _loot.AddArmor(builder.Build(1)); // todo use area level
        }
        return this;
    }

    public AreaBuilder DefineEnemy(Func<Character> definition)
    {
        _enemies.Add(definition);
        return this;
    }

    public AreaBuilder WithMap(Map map)
    {
        _map = map;
        return this;
    }

    public AreaBuilder WithExit(IExit exit)
    {
        _exits.Add(exit);
        return this;
    }

    // todo move to extension method once skills are added
    public AreaBuilder AddMiningSkill()
    {
        var bronze = new Material(new ItemType("Bronze", Grade.UNCOMMON), "Used to craft low-level melee equipment");
        var result = this
            .DefineMaterial(new Material(new ItemType("Stone", Grade.COMMON), "A low-grade mining material, but it's better than nothing."))
            .DefineMaterial(bronze)
            .DefineMaterial(new Material(new ItemType("Silver", Grade.RARE), "Used to craft enhancements for your equipment."))
            .DefineMaterial(new Material(new ItemType("Gold", Grade.EPIC), "Used to craft powerful magical artifacts."))
            .DefineMaterial(new Material(new ItemType("Titanium", Grade.LEGENDARY), "A high-grade mining material for crafting powerful melee equipment."))
            ;

        var khopeshBuilder = new WeaponBuilder(new ItemType("Bronze Khopesh", Grade.UNCOMMON))
            .WithRange(AttackRange.MELEE)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.OFFENSE, 2},
                {PlayerStatType.MAX_HIT_POINTS, 1}
            });
        result = result.DefineWeapon(khopeshBuilder, false);

        var khopeshRecipe = new CraftingRecipeBuilder()
            .And(bronze, 25)
            .Makes(() => ItemUnion.Of(khopeshBuilder.Build(1))); // craft at level 1 so players don't just grind lv 20 weapons in lv 1 area
        _gameBuilder.WithCraftingRecipe(khopeshRecipe);

        var bronzeArmorBuilder = new ArmorBuilder(new ItemType("Bronze Armor", Grade.UNCOMMON))
            .WithArmorClass(ArmorClass.HEAVY)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.OFFENSE, 1},
                {PlayerStatType.MAX_HIT_POINTS, 2},
                {PlayerStatType.MAX_ENERGY, 1}
            });
        result = result.DefineArmor(bronzeArmorBuilder, false);

        var bronzeArmorRecipe = new CraftingRecipeBuilder()
            .And(bronze, 25)
            .Makes(() => ItemUnion.Of(bronzeArmorBuilder.Build(1)));
        _gameBuilder.WithCraftingRecipe(bronzeArmorRecipe);

        return result;
    }

    public AreaBuilder AddStarterEquipment()
    {
        var result = this 
            .DefineWeapon(new WeaponBuilder(new ItemType("Training Sword", Grade.COMMON))
                .WithRange(AttackRange.MELEE)
                .WithStatWeights(new Dictionary<PlayerStatType, int>()
                {
                    {PlayerStatType.OFFENSE, 1},
                    {PlayerStatType.MAX_HIT_POINTS, 1}
                }), true
            )
            .DefineWeapon(new WeaponBuilder(new ItemType("Training Bow", Grade.COMMON))
                .WithRange(AttackRange.LONG)
                .WithStatWeights(new Dictionary<PlayerStatType, int>()
                {
                    {PlayerStatType.OFFENSE, 1},
                    {PlayerStatType.SPEED, 1}
                }), true
            )
            .DefineWeapon(new WeaponBuilder(new ItemType("Training Staff", Grade.COMMON))
                .WithRange(AttackRange.MEDIUM)
                .WithStatWeights(new Dictionary<PlayerStatType, int>()
                {
                    {PlayerStatType.MAX_HIT_POINTS, 1},
                    {PlayerStatType.MAX_ENERGY, 1}
                }), true
            )
            .DefineArmor(new ArmorBuilder(new ItemType("Fighter Initiate's Armor", Grade.COMMON))
                .WithArmorClass(ArmorClass.HEAVY)
                .WithStatWeights(new Dictionary<PlayerStatType, int>()
                {
                    {PlayerStatType.MAX_HIT_POINTS, 1},
                    {PlayerStatType.OFFENSE, 1}
                }), true
            )
            .DefineArmor(new ArmorBuilder(new ItemType("Archer Initiate's Armor", Grade.COMMON))
                .WithArmorClass(ArmorClass.MEDIUM)
                .WithStatWeights(new Dictionary<PlayerStatType, int>()
                {
                    {PlayerStatType.SPEED, 1},
                    {PlayerStatType.OFFENSE, 1}
                }), true
            )
            .DefineArmor(new ArmorBuilder(new ItemType("Mage Initiate's Armor", Grade.COMMON))
                .WithArmorClass(ArmorClass.LIGHT)
                .WithStatWeights(new Dictionary<PlayerStatType, int>()
                {
                    {PlayerStatType.MAX_ENERGY, 1},
                    {PlayerStatType.OFFENSE, 1}
                }), true
            );
        return result;
    }

    public AreaBuilder AddDefaultEnemies()
    {
        var result = this
            .DefineEnemy(() => new Character("Snake", 1, Color.LIGHT_GREEN))
            .DefineEnemy(() => new Character("Scorpion", 1, Color.BLACK))
            .DefineEnemy(() => new Character("Jackal", 2, Color.TAN));
        return result;
    }

    public Area Build(IGame game)
    {
        if (_map == null)
        {
            throw new Exception("map is required");
        }
        var result = new Area(
            Name,
            game,
            _map,
            _loot,
            _enemies,
            _exits
        );
        result.AddTimer(new GameTimer(result.SpawnLootChest, Duration.FromSeconds(10).InFrames));
        var spawner = new EnemySpawner(result, new EnemySpawnerOptions()
        {
            WaveSize = 3,
            MaxChildren = 10
        });
        result.AddTimer(new GameTimer(spawner.SpawnWave, Duration.FromSeconds(10).InFrames));

        return result;
    }
}
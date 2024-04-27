using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Dynamic.Player;
using GearBox.Core.Model.Stable.Items;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model;

public class WorldBuilder
{
    private Map? _map;
    private readonly List<ItemType> _itemTypes = [];
    private readonly LootTable _loot = new();
    private readonly List<Func<Character>> _enemies = [];


    public WorldBuilder DefineMaterial(string name, string description, Grade grade)
    {
        var type = new ItemType(name, grade);
        _loot.AddMaterial(new Material(type, description));
        _itemTypes.Add(type);
        return this;
    }

    public WorldBuilder DefineWeapon(string name, Grade grade, Action<WeaponBuilder> modifyBuilder)
    {
        var itemType = new ItemType(name, grade);
        var builder = new WeaponBuilder(itemType);
        modifyBuilder(builder);
        _loot.AddWeapon(builder.Build(1)); // in the future, this will be based on the area level
        _itemTypes.Add(itemType);
        return this;
    }

    // todo move to extension method once skills are added
    public WorldBuilder AddMiningSkill()
    {
        var result = this
            .DefineMaterial("Stone", "A low-grade mining material, but it's better than nothing.", Grade.COMMON)
            .DefineMaterial("Bronze", "Used to craft low-level melee equipment.", Grade.UNCOMMON)
            .DefineMaterial("Silver", "Used to craft enhancements for your equipment.", Grade.RARE)
            .DefineMaterial("Gold", "Used to craft powerful magical artifacts.", Grade.EPIC)
            .DefineMaterial("Titanium", "A high-grade mining material for crafting powerful melee equipment.", Grade.LEGENDARY);
        
        return result;
    }

    public WorldBuilder AddStarterWeapons()
    {
        var result = this 
            .DefineWeapon("Training Sword", Grade.COMMON, builder => builder
                .WithDescription("No special ability")
                .WithRange(AttackRange.MELEE)
                .WithStatWeights(weights => weights
                    .Weigh(PlayerStatType.OFFENSE, 1)
                    .Weigh(PlayerStatType.DEFENSE, 1)
                )
            )
            .DefineWeapon("Training Bow", Grade.COMMON, builder => builder
                .WithDescription("Hit stuff from far away.")
                .WithRange(AttackRange.LONG)
                .WithStatWeights(weights => weights
                    .Weigh(PlayerStatType.OFFENSE, 1)
                    .Weigh(PlayerStatType.SPEED, 1)
                )
            )
            .DefineWeapon("Training Staff", Grade.COMMON, builder => builder
                .WithDescription("Also no special ability.")
                .WithRange(AttackRange.MEDIUM)
                .WithStatWeights(weights => weights
                    .Weigh(PlayerStatType.DEFENSE, 1)
                    .Weigh(PlayerStatType.MAX_ENERGY, 1)
                )
            );
        return result;
    }

    public WorldBuilder DefineEnemy(Func<Character> definition)
    {
        _enemies.Add(definition);
        return this;
    }

    public WorldBuilder AddDefaultEnemies()
    {
        var result = this
            .DefineEnemy(() => new Character("Snake", 1))
            .DefineEnemy(() => new Character("Scorpion", 1))
            .DefineEnemy(() => new Character("Jackal", 2));
        return result;
    }

    public WorldBuilder WithDesertMap()
    {
        // for now, I won't read from a CSV file, as it is difficult to ensure subprojects can find the right file
        int[,] csv = {
        //   0              5              10             15
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3}, // 0
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 3},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2, 2, 2, 2, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2, 2, 2, 2, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0},
            {0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0}, // 5
            {0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 3, 3, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0}, // 10
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 2}, // 15
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 2, 2},
            {0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 2, 2, 0, 0, 2, 2, 2},
            {3, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2},
            {3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2}
        };
        _map = new Map(Dimensions.InTiles(20))
            .SetTileTypeForKey(0, TileType.Intangible(Color.TAN))
            .SetTileTypeForKey(1, TileType.Tangible(Color.GRAY))
            .SetTileTypeForKey(2, TileType.Tangible(Color.BLUE)) // water
            .SetTileTypeForKey(3, TileType.Intangible(Color.LIGHT_GREEN)) // plants
            .SetTilesFrom(csv);
        return this;
    }

    public World Build()
    {
        if (_map == null)
        {
            throw new Exception();
        }
        var result = new World(
            Guid.NewGuid(),
            _map,
            ItemTypeRepository.Of(_itemTypes),
            _loot,
            _enemies
        );
        return result;
    }
}
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model;

public class WorldBuilder
{
    private Map? _map;
    private readonly HashSet<ItemType> _itemTypes = [];
    private readonly HashSet<CraftingRecipe> _craftingRecipes = [];
    private readonly LootTable _loot = new();
    private readonly List<Func<Character>> _enemies = [];


    public WorldBuilder DefineMaterial(Material material)
    {
        _itemTypes.Add(material.Type);
        _loot.AddMaterial(material); // all materials are loot for now
        return this;
    }

    public WorldBuilder DefineWeapon(WeaponBuilder builder, bool isLoot)
    {
        _itemTypes.Add(builder.ItemType);
        if (isLoot)
        {
            _loot.AddWeapon(builder.Build(1)); // todo use area level
        }
        return this;
    }

    public WorldBuilder DefineEnemy(Func<Character> definition)
    {
        _enemies.Add(definition);
        return this;
    }

    // todo move to extension method once skills are added
    public WorldBuilder AddMiningSkill()
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
            .WithDescription("An ancient weapon for the modern age.")
            .WithRange(AttackRange.MELEE)
            .WithStatWeights(weights => weights
                .Weigh(PlayerStatType.OFFENSE, 2)
                .Weigh(PlayerStatType.MAX_HIT_POINTS, 1)
            );
        result = result.DefineWeapon(khopeshBuilder, false);

        var khopeshRecipe = new CraftingRecipeBuilder()
            .And(bronze, 25)
            .Makes(() => ItemUnion.Of(khopeshBuilder.Build(1))); // what level should the crafted weapon be?
        _craftingRecipes.Add(khopeshRecipe);

        return result;
    }

    public WorldBuilder AddStarterWeapons()
    {
        var result = this 
            .DefineWeapon(new WeaponBuilder(new ItemType("Training Sword", Grade.COMMON))
                .WithDescription("No special ability")
                .WithRange(AttackRange.MELEE)
                .WithStatWeights(weights => weights
                    .Weigh(PlayerStatType.OFFENSE, 1)
                    .Weigh(PlayerStatType.MAX_HIT_POINTS, 1)
                ), true
            )
            .DefineWeapon(new WeaponBuilder(new ItemType("Training Bow", Grade.COMMON))
                .WithDescription("Hit stuff from far away.")
                .WithRange(AttackRange.LONG)
                .WithStatWeights(weights => weights
                    .Weigh(PlayerStatType.OFFENSE, 1)
                    .Weigh(PlayerStatType.SPEED, 1)
                ), true
            )
            .DefineWeapon(new WeaponBuilder(new ItemType("Training Staff", Grade.COMMON))
                .WithDescription("Also no special ability.")
                .WithRange(AttackRange.MEDIUM)
                .WithStatWeights(weights => weights
                    .Weigh(PlayerStatType.MAX_HIT_POINTS, 1)
                    .Weigh(PlayerStatType.MAX_ENERGY, 1)
                ), true
            );
        return result;
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
            CraftingRecipeRepository.Of(_craftingRecipes),
            _loot,
            _enemies
        );
        return result;
    }
}
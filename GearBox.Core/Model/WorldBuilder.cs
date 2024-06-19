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

    public WorldBuilder DefineWeapon(EquipmentBuilder<Weapon> builder, bool isLoot)
    {
        _itemTypes.Add(builder.ItemType);
        if (isLoot)
        {
            _loot.AddWeapon(builder.Build(1)); // todo use area level
        }
        return this;
    }

    public WorldBuilder DefineArmor(EquipmentBuilder<Armor> builder, bool isLoot)
    {
        _itemTypes.Add(builder.ItemType);
        if (isLoot)
        {
            _loot.AddArmor(builder.Build(1)); // todo use area level
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
        _craftingRecipes.Add(khopeshRecipe);

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
        _craftingRecipes.Add(bronzeArmorRecipe);

        return result;
    }

    public WorldBuilder AddStarterEquipment()
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
            .SetTileTypeForKey(0, new(Color.TAN, TileHeight.FLOOR))
            .SetTileTypeForKey(1, new(Color.GRAY, TileHeight.WALL))
            .SetTileTypeForKey(2, new(Color.BLUE, TileHeight.PIT)) // water
            .SetTileTypeForKey(3, new(Color.LIGHT_GREEN, TileHeight.FLOOR)) // plants
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
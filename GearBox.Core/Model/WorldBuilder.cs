using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Stable.Items;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model;

public class WorldBuilder
{
    private Map? _map;
    private readonly List<ItemType> _itemTypes = new();
    private readonly LootTable _loot = new();

    public WorldBuilder DefineItem(ItemDefinition itemDefinition)
    {
        _loot.Add(itemDefinition);
        _itemTypes.Add(itemDefinition.Type);
        return this;
    }

    public WorldBuilder DefineMaterial(string name, string description, Grade grade)
    {
        var itemDefinition = new ItemDefinition(new ItemType(name, grade), t => new Material(t, description));
        return DefineItem(itemDefinition);
    }

    public WorldBuilder DefineWeapon(string name, Grade grade, Action<WeaponBuilder> modifyBuilder)
    {
        var itemType = new ItemType(name, grade);
        var builder = new WeaponBuilder(itemType);
        modifyBuilder(builder);
        var itemDefinition = new ItemDefinition(itemType, _ => builder.Build(1)); // in the future, this will be based on the area level
        return DefineItem(itemDefinition);
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

    public WorldBuilder WithDummyMap()
    {
        _map = new Map();
        _map.SetTileTypeForKey(1, TileType.Tangible(Color.RED));
        _map.SetTileAt(Coordinates.FromTiles(5, 5), 1);
        _map.SetTileAt(Coordinates.FromTiles(5, 6), 1);
        _map.SetTileAt(Coordinates.FromTiles(6, 5), 1);
        _map.SetTileAt(Coordinates.FromTiles(8, 5), 1);
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
            new StaticWorldContent(_map, new List<IStaticGameObject>()),
            ItemTypeRepository.Of(_itemTypes),
            _loot
        );
        return result;
    }
}
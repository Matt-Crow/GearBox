using GearBox.Core.Model.Stable;
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
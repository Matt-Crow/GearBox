using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model;

public class WorldBuilder
{
    private Map? _map;
    private readonly List<ItemType> _itemTypes = new();
    private readonly LootTable _loot = new();

    public WorldBuilder DefineItem(Func<IItem> definition)
    {
        _loot.Add(definition);
        _itemTypes.Add(definition.Invoke().Type);
        return this;
    }

    public WorldBuilder AddDummyItems()
    {
        DefineItem(() => new Material(new ItemType("Stone")));
        DefineItem(() => new Material(new ItemType("Wood")));
        DefineItem(() => new Equipment(new ItemType("Rusty Shovel")));
        return this;
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
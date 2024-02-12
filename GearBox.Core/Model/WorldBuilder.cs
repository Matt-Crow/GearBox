using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model;

public class WorldBuilder
{
    private Map? _map;
    private readonly List<ItemType> _itemTypes = new();

    public WorldBuilder AddItemType(ItemType itemType)
    {
        _itemTypes.Add(itemType);
        return this;
    }

    public WorldBuilder AddItemTypes(IEnumerable<ItemType> itemTypes)
    {
        var result = this;
        foreach (var itemType in itemTypes)
        {
            result = result.AddItemType(itemType);
        }
        return result;
    }

    public WorldBuilder AddDummyItemTypes()
    {
        var dummyItemTypes = new List<ItemType>()
        {
            new ItemType("Wood"),
            new ItemType("Rock"),
            new ItemType("Rusty Shovel")
        };
        return AddItemTypes(dummyItemTypes);
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
            ItemTypeRepository.Of(_itemTypes)
        );
        return result;
    }
}
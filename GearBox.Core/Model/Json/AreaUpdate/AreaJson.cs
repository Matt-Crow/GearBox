namespace GearBox.Core.Model.Json.AreaUpdate;

public class AreaJson : IChange
{
    public AreaJson(string name, MapJson map, List<ShopInitJson> shops)
    {
        Name = name;
        Map = map;
        Shops = shops;
    }

    public string Name { get; init; }
    public MapJson Map { get; init; }
    public List<ShopInitJson> Shops { get; init; }

    public IEnumerable<object?> DynamicValues => [Name];
}
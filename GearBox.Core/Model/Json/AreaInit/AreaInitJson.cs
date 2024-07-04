namespace GearBox.Core.Model.Json.AreaInit;

/// <summary>
/// Sent as a message to users upon first connecting to an area.
/// It contains all information which will never change, and thus only needs to
/// be sent once.
/// </summary>
public readonly struct AreaInitJson : IJson
{
    public AreaInitJson(MapJson map)
    {
        Map = map;
    }

    public MapJson Map { get; init; }
}
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Areas;

/// <summary>
/// An exit which triggers when a player reaches a border of the map
/// </summary>
public class BorderExit : IExit
{
    private readonly Func<BodyBehavior, Dimensions, bool> _isPastBorder;
    private readonly Action<BodyBehavior, Dimensions> _onExit;

    private BorderExit(string destinationName, Func<BodyBehavior, Dimensions, bool> isPastBorder, Action<BodyBehavior, Dimensions> onExit)
    {
        DestinationName = destinationName;
        _isPastBorder = isPastBorder;
        _onExit = onExit;
    }

    public static BorderExit Left(string destinationName) => new(
        destinationName,
        (body, bounds) => body.LeftInPixels < 0,
        (body, bounds) => body.RightInPixels = bounds.WidthInPixels
    );
    public static BorderExit Right(string destinationName) => new(
        destinationName,
        (body, bounds) => body.RightInPixels > bounds.WidthInPixels,
        (body, bounds) => body.LeftInPixels = 0
    );
    public static BorderExit Top(string destinationName) => new(
        destinationName,
        (body, bounds) => body.TopInPixels < 0,
        (body, bounds) => body.BottomInPixels = bounds.HeightInPixels
    );
    public static BorderExit Bottom(string destinationName) => new(
        destinationName,
        (body, bounds) => body.BottomInPixels > bounds.HeightInPixels,
        (body, bounds) => body.TopInPixels = 0
    );

    public string DestinationName { get; init; }

    public bool ShouldExit(PlayerCharacter player, IArea area) => _isPastBorder(player.Body, area.Bounds);

    public void OnExit(PlayerCharacter player, IArea area) => _onExit(player.Body, area.Bounds);
}
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Static;

public class TileIterator
{
    private readonly Map _map;
    private readonly Coordinates _start;
    private readonly Dimensions _area;
    private int _currentX;
    private int _currentY;

    public TileIterator(Map map, Coordinates start, Dimensions area)
    {
        _map = map;
        _start = start;
        _area = area;
        _currentX = start.XInTiles;
        _currentY = start.YInTiles;
    }

    private Coordinates CurrentPoint { get => Coordinates.FromTiles(_currentX, _currentY); }
    public Tile Current { get => new(CurrentPoint, _map.GetTileAt(CurrentPoint)); }
    public bool Done { get; private set; } = false;

    public void Next()
    {
        if (Done)
        {
            return;
        }

        if (_currentX == _start.XInTiles + _area.WidthInTiles - 1)
        {
            // finished row
            _currentX = _start.XInTiles;
            _currentY++;
            Done = _currentY == _start.YInTiles + _area.HeightInTiles;
        }
        else
        {
            // next column
            _currentX++;
        }

        if (!Done && !_map.IsValid(CurrentPoint))
        {
            Next(); // skip invalid coordinates
        }
    }
}
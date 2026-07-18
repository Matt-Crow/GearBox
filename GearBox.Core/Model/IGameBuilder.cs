using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model;

public interface IGameBuilder
{
    /// <summary>
    /// A reference to the items available in the game this is building.
    /// </summary>
    IItemFactory Items { get;}

    /// <summary>
    /// Defines an area in the game. The name must be unique within the game.
    /// </summary>
    IGameBuilder WithArea(string name, int level, Func<AreaBuilder, AreaBuilder> defineArea);
    
    IGame Build();
}
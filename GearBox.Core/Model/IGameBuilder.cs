using GearBox.Core.Model.Items;
using GearBox.Core.Model.ResourcePacks;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model;

public interface IGameBuilder
{
    /// <summary>
    /// Makes the items available in the game this is building.
    /// </summary>
    Factory<ItemUnion> Items { get; }

    /// <summary>
    /// Defines an area in the game. The name must be unique within the game.
    /// </summary>
    IGameBuilder WithArea(AreaResource area);
    
    IGame Build();
}
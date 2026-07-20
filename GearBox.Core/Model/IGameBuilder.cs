using GearBox.Core.Model.Items;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model;

public interface IGameBuilder
{
    /// <summary>
    /// Makes the items available in the game this is building.
    /// </summary>
    Factory<ItemUnion> Items { get; }
    
    IGame Build();
}
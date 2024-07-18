using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;

namespace GearBox.Core.Model;

public interface IGameBuilder
{
    IGameBuilder WithItemType(ItemType itemType);
    IGameBuilder WithCraftingRecipe(CraftingRecipe recipe);

    /// <summary>
    /// Defines an area in the game. The name must be unique within the game.
    /// </summary>
    IGameBuilder WithArea(string name, Func<AreaBuilder, AreaBuilder> defineArea);
    
    IGame Build();
}
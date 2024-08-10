using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model;

public interface IGameBuilder
{
    IGameBuilder DefineItems(Func<IItemFactory, IItemFactory> defineItems);
    IGameBuilder AddCraftingRecipe(Func<CraftingRecipeBuilder, CraftingRecipe> recipe);

    /// <summary>
    /// Defines an area in the game. The name must be unique within the game.
    /// </summary>
    IGameBuilder WithArea(string name, int level, Func<AreaBuilder, AreaBuilder> defineArea);
    
    IGame Build();
}
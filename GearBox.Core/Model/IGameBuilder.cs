using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;

namespace GearBox.Core.Model;

public interface IGameBuilder
{
    IGameBuilder WithItemType(ItemType itemType);
    IGameBuilder WithCraftingRecipe(CraftingRecipe recipe);
    IGameBuilder WithArea(Func<WorldBuilder, WorldBuilder> defineArea);
    IGame Build();
}
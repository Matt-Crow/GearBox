using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;

namespace GearBox.Core.Model;

public interface IGameBuilder
{
    IGameBuilder WithItemType(ItemType itemType);
    IGameBuilder WithCraftingRecipe(CraftingRecipe recipe);
    IGameBuilder WithArea(Func<AreaBuilder, AreaBuilder> defineArea);
    IGame Build();
}
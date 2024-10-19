using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model;

public interface IGameBuilder
{
    IGameBuilder DefineActiveAbilities(Func<IActiveAbilityFactory, IActiveAbilityFactory> defineActives);
    IGameBuilder DefineItems(Func<IItemFactory, IItemFactory> defineItems);
    IGameBuilder DefineEnemies(Func<IEnemyRepository, IEnemyRepository> defineEnemies);
    IGameBuilder AddCraftingRecipe(Func<CraftingRecipeBuilder, CraftingRecipe> recipe);

    /// <summary>
    /// Defines an area in the game. The name must be unique within the game.
    /// </summary>
    IGameBuilder WithArea(string name, int level, Func<AreaBuilder, AreaBuilder> defineArea);
    
    IGame Build();
}
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json.GameInit;

namespace GearBox.Core.Model;

/// <summary>
/// top level model in which everything happens
/// </summary>
public interface IGame
{
    CraftingRecipe? GetCraftingRecipeById(Guid id);

    void AddArea(IArea area);

    IArea GetDefaultArea();

    IArea? GetAreaByName(string name);

    GameInitJson GetGameInitJsonFor(PlayerCharacter player);

    void Update();
}
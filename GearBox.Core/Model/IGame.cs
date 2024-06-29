using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Json.AreaInit;

namespace GearBox.Core.Model;

/// <summary>
/// top level model in which everything happens
/// </summary>
public interface IGame
{
    /// <summary>
    /// this will no longer be needed once GameInit is implemented
    /// </summary>
    /// <returns></returns>
    List<ItemTypeJson> GetItemTypeJsons();

    /// <summary>
    /// the will no longer be needed once GameInit is implemented
    /// </summary>
    List<CraftingRecipeJson> GetCraftingRecipeJsons();

    CraftingRecipe? GetCraftingRecipeById(Guid id);

    void AddArea(IArea area);

    IArea GetDefaultArea();
}
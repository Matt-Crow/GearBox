using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

public class Craft : IControlCommand
{
    private readonly Guid _recipeId;

    public Craft(Guid recipeId)
    {
        _recipeId = recipeId;
    }

    public void ExecuteOn(PlayerCharacter target, World inWorld)
    {
        var recipe = inWorld.CraftingRecipes.GetById(_recipeId);
        if (recipe != null)
        {
            target.Inventory.Craft(recipe);
        }
    }
}

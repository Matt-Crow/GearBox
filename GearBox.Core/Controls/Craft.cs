using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

public class Craft : IControlCommand
{
    private readonly Guid _recipeId;

    public Craft(Guid recipeId)
    {
        _recipeId = recipeId;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        // this will change once recipes are stored in an aggregate of areas
        var area = target.CurrentArea ?? throw new Exception("Cannot craft when not in an area");
        area
            .Game
            .Crafter
            .Craft(_recipeId, target.Inventory);
    }
}

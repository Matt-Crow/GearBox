using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items.Crafting;

namespace GearBox.Core.Model.Areas;

/// <summary>
/// An area is where game objects can exist.
/// Things inside an area cannot interact with things in a different area.
/// </summary>
public interface IArea
{
    /// <summary>
    /// Spawns a player into the area an heals them back to full,
    /// if they are not already in the area
    /// </summary>
    void SpawnPlayer(PlayerCharacter player);

    Character SpawnEnemy();
    
    void AddProjectile(Projectile projectile);

    CraftingRecipe? GetCraftingRecipeById(Guid id);

    Character? GetNearestEnemy(Character character);
}
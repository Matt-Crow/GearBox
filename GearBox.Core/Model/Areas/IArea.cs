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
    /// Spawns a player into the world an heals them back to full,
    /// if they are not already in the world
    /// </summary>
    void SpawnPlayer(PlayerCharacter player);
    
    void AddProjectile(Projectile projectile);

    CraftingRecipe? GetCraftingRecipeById(Guid id);

    /// <summary>
    /// Finds the nearest character who is not on the same team as the given character.
    /// Returns null if no such character can be found.
    /// </summary>
    Character? GetNearestEnemy(Character character);
}
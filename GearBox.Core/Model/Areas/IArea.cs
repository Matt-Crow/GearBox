using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json.AreaInit;
using GearBox.Core.Model.Json.AreaUpdate;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Areas;

/// <summary>
/// An area is where game objects can exist.
/// Things inside an area cannot interact with things in a different area.
/// </summary>
public interface IArea
{
    /// <summary>
    /// An identifier for this area, unique to the game it occupies
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Spawns a player into the area an heals them back to full,
    /// if they are not already in the area
    /// </summary>
    void SpawnPlayer(PlayerCharacter player);

    Character SpawnEnemy();
    void SpawnLootChest();
    
    void AddProjectile(Projectile projectile);
    void AddTimer(GameTimer timer);

    void RemovePlayer(PlayerCharacter player);

    CraftingRecipe? GetCraftingRecipeById(Guid id);

    Character? GetNearestEnemy(Character character);

    Coordinates GetRandomFloorTile();

    MapJson GetMapJson();

    AreaUpdateJson GetAreaUpdateJsonFor(PlayerCharacter player);
    
    /// <summary>
    /// Called each game tick.
    /// Updates the area and everything in it
    /// </summary>
    void Update();
}
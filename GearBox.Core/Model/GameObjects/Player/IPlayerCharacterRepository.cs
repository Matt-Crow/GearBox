namespace GearBox.Core.Model.GameObjects.Player;

/// <summary>
/// Loads and stores player characters in a backing store
/// </summary>
public interface IPlayerCharacterRepository
{
    /// <summary>
    /// Returns the player character with the given ASP.NET Identity user ID from the backing store,
    /// or null if no such player character exists.
    /// May throw an exception if any database issues occur.
    /// </summary>
    Task<PlayerCharacter?> GetPlayerCharacterByAspNetUserIdAsync(string aspNetUserId);

    /// <summary>
    /// Adds or updates the given player character in the database.
    /// May throw an exception if an database issues occur.
    /// </summary>
    Task SavePlayerCharacterAsync(PlayerCharacter playerCharacter, string aspNetUserId);
}
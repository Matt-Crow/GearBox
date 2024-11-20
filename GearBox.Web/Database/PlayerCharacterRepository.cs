using GearBox.Core.Model.GameObjects.Player;
using Microsoft.EntityFrameworkCore;

namespace GearBox.Web.Database;

public class PlayerCharacterRepository : IPlayerCharacterRepository
{
    private readonly IDbContextFactory<GameDbContext> _dbContextFactory;

    public PlayerCharacterRepository(IDbContextFactory<GameDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public Task<PlayerCharacter?> GetPlayerCharacterByAspNetUserIdAsync(string aspNetUserId)
    {
        throw new NotImplementedException();
    }

    public async Task SavePlayerCharacterAsync(PlayerCharacter playerCharacter, string aspNetUserId)
    {
        using var db = _dbContextFactory.CreateDbContext();
        var existing = await db.PlayerCharacters
            .Where(p => p.Id == playerCharacter.Id)
            .FirstOrDefaultAsync();
        if (existing == null)
        {
            // insert
            var dbModel = DbPlayerCharacter.FromGameModel(playerCharacter, aspNetUserId);
            db.PlayerCharacters.Add(dbModel);
        }
        else
        {
            // update
            existing.UpdateFrom(playerCharacter);
            db.PlayerCharacters.Update(existing);
        }
        
        await db.SaveChangesAsync();
    }
}

using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GearBox.Web.Database;

public class PlayerCharacterRepository : IPlayerCharacterRepository
{
    private readonly IDbContextFactory<GearBoxDbContext> _dbContextFactory;
    private readonly IItemFactory _itemFactory;

    public PlayerCharacterRepository(IDbContextFactory<GearBoxDbContext> dbContextFactory, IItemFactory itemFactory)
    {
        _dbContextFactory = dbContextFactory;
        _itemFactory = itemFactory;
    }

    public async Task<PlayerCharacter?> GetPlayerCharacterByAspNetUserIdAsync(string aspNetUserId)
    {
        using var db = _dbContextFactory.CreateDbContext();
        var dbModel = await db.PlayerCharacters.AsNoTracking()
            .Where(p => p.AspNetUserId == aspNetUserId)
            .Include(p => p.Items)
            .FirstOrDefaultAsync();

        // convert from database model
        var gameModel = dbModel?.ToGameModel(_itemFactory);

        return gameModel;
    }

    public async Task SavePlayerCharacterAsync(PlayerCharacter playerCharacter, string aspNetUserId)
    {
        using var db = _dbContextFactory.CreateDbContext();
        var existing = await db.PlayerCharacters
            .Where(p => p.Id == playerCharacter.Id)
            .Include(p => p.Items)
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

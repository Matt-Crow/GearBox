using Microsoft.EntityFrameworkCore;

namespace GearBox.Web.Database;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<DbPlayerCharacter> PlayerCharacters { get; set; }
}
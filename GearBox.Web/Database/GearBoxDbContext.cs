using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GearBox.Web.Database;

/// <summary>
/// Database context for both the game model and ASP.NET Identity.
/// </summary>
public class GearBoxDbContext(DbContextOptions<GearBoxDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public required DbSet<DbPlayerCharacter> PlayerCharacters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // must come before customization

        modelBuilder.Entity<DbPlayerCharacter>(e => {
            e.OwnsMany(pc => pc.PartSlots, es =>
            {
                // set up a composite unique constraint on player character ID & slot type
                es
                    .HasIndex(e => new { e.PlayerCharacterId, e.SlotType })
                    .IsUnique();
            });
        });
    }
}
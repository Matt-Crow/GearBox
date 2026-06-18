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
            e.OwnsOne(pc => pc.EquippedManipulator, ew => {
                ew.Property(e => e.Name).HasColumnName("equipped_manipulator_name");
                ew.Property(e => e.Level).HasColumnName("equipped_manipulator_level");
            });
            e.OwnsOne(pc => pc.EquippedTorso, ea => {
                ea.Property(e => e.Name).HasColumnName("equipped_torso_name");
                ea.Property(e => e.Level).HasColumnName("equipped_torso_level");
            });
        });
    }
}
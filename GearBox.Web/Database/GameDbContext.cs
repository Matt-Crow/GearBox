using Microsoft.EntityFrameworkCore;

namespace GearBox.Web.Database;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public required DbSet<DbPlayerCharacter> PlayerCharacters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbPlayerCharacter>(e => {
            e.OwnsOne(pc => pc.EquippedWeapon, ew => {
                ew.Property(e => e.Name).HasColumnName("equipped_weapon_name");
                ew.Property(e => e.Level).HasColumnName("equipped_weapon_level");
            });
            e.OwnsOne(pc => pc.EquippedArmor, ea => {
                ea.Property(e => e.Name).HasColumnName("equipped_armor_name");
                ea.Property(e => e.Level).HasColumnName("equipped_armor_level");
            });
        });
    }
}
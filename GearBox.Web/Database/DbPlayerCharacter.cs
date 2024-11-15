using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GearBox.Web.Database;

[Table("gb_player_character")]
public class DbPlayerCharacter
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("asp_net_user_id")]
    public Guid AspNetUserId { get; set; }

    [Required]
    [Column("name")]
    public required string Name { get; set; }

    [Required]
    [Column("xp")]
    public int Xp { get; set; }

    [Required]
    [Column("gold")]
    public int Gold { get; set; }

    [Column("equipped_weapon_id")]
    public Guid? EquippedWeaponId { get; set; }

    [Column("equipped_armor_id")]
    public Guid? EquippedArmorId { get; set; }
}
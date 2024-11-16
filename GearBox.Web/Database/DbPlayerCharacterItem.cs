using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GearBox.Web.Database;

[Table("gb_player_character_item")]
public class DbPlayerCharacterItem
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("player_character_id")]
    public Guid PlayerCharacterId { get; set; }

    [Required]
    [Column("name")]
    public required string Name { get; set; }

    [Required]
    [Column("level")]
    public int Level { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [ForeignKey(nameof(PlayerCharacterId))]
    public required virtual DbPlayerCharacter PlayerCharacter { get; set;}
}
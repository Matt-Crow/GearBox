using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GearBox.Core.Model.Items;

namespace GearBox.Web.Database;

[Table("gb_player_character_part_slot")]
public class DbPlayerCharacterPartSlot
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("player_character_id")]
    public required Guid PlayerCharacterId { get; set; }

    [Required]
    [Column("slot_type")]
    public required string SlotType { get; set; }

    [Column("part_name")]
    public string? PartName { get; set; }

    [Column("part_level")]
    public int? PartLevel { get; set; }

    [ForeignKey(nameof(PlayerCharacterId))]
    public required virtual DbPlayerCharacter PlayerCharacter { get; set; }

    /// <summary>
    /// Converts from the game model to the database model
    /// </summary>
    public static DbPlayerCharacterPartSlot FromGameModel(DbPlayerCharacter parent, PartSlot gameModel)
    {
        var dbModel = new DbPlayerCharacterPartSlot()
        {
            //Id = gameModel.Id ?? new Guid(), setting Id makes EFCore think it already exists in the DB, but it doesn't
            PlayerCharacterId = parent.Id,
            SlotType = gameModel.SlotType.Name,
            PartName = gameModel.Part?.Name,
            PartLevel = gameModel.Part?.Level,
            PlayerCharacter = parent
        };
        return dbModel;
    }
}
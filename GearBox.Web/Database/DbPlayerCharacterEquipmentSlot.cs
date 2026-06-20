using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GearBox.Core.Model.Items;

namespace GearBox.Web.Database;

[Table("gb_player_character_equipment_slot")]
public class DbPlayerCharacterEquipmentSlot
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

    [Column("equipment_name")]
    public string? EquipmentName { get; set; }

    [Column("equipment_level")]
    public int? EquipmentLevel { get; set; }

    [ForeignKey(nameof(PlayerCharacterId))]
    public required virtual DbPlayerCharacter PlayerCharacter { get; set; }

    /// <summary>
    /// Converts from the game model to the database model
    /// </summary>
    public static DbPlayerCharacterEquipmentSlot FromGameModel(DbPlayerCharacter parent, EquipmentSlot gameModel)
    {
        var dbModel = new DbPlayerCharacterEquipmentSlot()
        {
            //Id = gameModel.Id ?? new Guid(), setting Id makes EFCore think it already exists in the DB, but it doesn't
            PlayerCharacterId = parent.Id,
            SlotType = gameModel.SlotType.Name,
            EquipmentName = gameModel.Equipment?.Name,
            EquipmentLevel = gameModel.Equipment?.Level,
            PlayerCharacter = parent
        };
        return dbModel;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Infrastructure;

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
    public required virtual DbPlayerCharacter PlayerCharacter { get; set; }

    /// <summary>
    /// Converts from the game model to the database model
    /// </summary>
    public static DbPlayerCharacterItem FromGameModel(DbPlayerCharacter parent, IItem gameModel, int level, int quantity)
    {
        var dbModel = new DbPlayerCharacterItem()
        {
            //Id = gameModel.Id ?? new Guid(), setting Id makes EFCore think it already exists in the DB, but it doesn't
            PlayerCharacterId = parent.Id,
            Name = gameModel.Name,
            Level = level,
            Quantity = quantity,
            PlayerCharacter = parent
        };
        return dbModel;
    }

    public IEnumerable<ItemUnion> ToGameModel(IItemFactory itemFactory)
    {
        // need to set ID?
        var gameModel = itemFactory.Make(Name) ?? throw new Exception($"Invalid item name: {Name}");
        for (var i = 0; i < Quantity; i++)
        {
            yield return gameModel.ToOwned(Level);
        }
    }
}
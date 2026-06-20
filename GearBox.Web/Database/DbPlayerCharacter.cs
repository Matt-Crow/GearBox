using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace GearBox.Web.Database;

[Table("gb_player_character")]
public class DbPlayerCharacter
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("asp_net_user_id")]
    public required string AspNetUserId { get; set; }

    [Required]
    [Column("name")]
    public required string Name { get; set; }

    [Required]
    [Column("xp")]
    public int Xp { get; set; }

    [Required]
    [Column("gold")]
    public int Gold { get; set; }

    public List<DbPlayerCharacterEquipmentSlot> EquipmentSlots { get; set; } = [];

    [ForeignKey(nameof(AspNetUserId))]
    public IdentityUser AspNetUser { get; set; } = null!;

    public required virtual ICollection<DbPlayerCharacterItem> Items { get; set; }

    /// <summary>
    /// Converts from the game model to the database model.
    /// </summary>
    public static DbPlayerCharacter FromGameModel(PlayerCharacter gameModel, string aspNetUserId)
    {
        var dbModel = new DbPlayerCharacter()
        {
            Id = gameModel.Id,
            AspNetUserId = aspNetUserId,
            Name = gameModel.Name,
            Xp = 0,
            Gold = 0,
            Items = []
        };
        dbModel.UpdateFrom(gameModel);

        return dbModel;
    }

    public void UpdateFrom(PlayerCharacter gameModel)
    {
        Xp = gameModel.Xp;
        Gold = gameModel.Inventory.Gold.Quantity;

        EquipmentSlots = gameModel.EquipmentSlots
            .Select(es => DbPlayerCharacterEquipmentSlot.FromGameModel(this, es))
            .ToList();
        
        Items.Clear();
        AddInventoryTab(gameModel.Inventory.Materials, i => 0);
        foreach (var equipmentTab in gameModel.Inventory.EquipmentTabs)
        {
            AddInventoryTab(equipmentTab, i => i.Level);
        }
    }

    private void AddInventoryTab<T>(InventoryTab<T> tab, Func<T, int> getLevel)
    where T : IItem
    {
        var converted = tab.Content.Select(stack => DbPlayerCharacterItem.FromGameModel(
            this,
            stack.Item,
            getLevel(stack.Item),
            stack.Quantity
        ));
        foreach (var item in converted)
        {
            Items.Add(item);
        }
    }

    public PlayerCharacter ToGameModel(IItemFactory itemFactory)
    {
        // don't need to store AspNetUserId in game model
        var result = new PlayerCharacter(Name, Xp, Id);
        result.Inventory.Add(new Gold(Gold));

        foreach (var dbItem in Items)
        {
            /*
                itemFactory.Make returns an ItemUnion,
                which has no way of storing quantity,
                so it would be confusing to put a ToGameModel method in DbPlayerCharacterItems
            */
            var gameItem = itemFactory.Make(dbItem.Name) ?? throw new Exception($"Invalid item name: {dbItem.Name}");
            result.Inventory.Add(gameItem.ToOwned(dbItem.Level), dbItem.Quantity);
        }

        foreach (var equipmentSlot in EquipmentSlots)
        {
            if (equipmentSlot.EquipmentName != null)
            {
                var gameItem = itemFactory.Make(equipmentSlot.EquipmentName) ?? throw new Exception($"Invalid item name: {equipmentSlot.EquipmentName}");
                result.Inventory.Add(gameItem);
                result.EquipById(gameItem.Id ?? throw new Exception("Item must have ID"));
            }
        }

        return result;
    }
}
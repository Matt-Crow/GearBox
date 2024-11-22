using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Infrastructure;

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

    [Column("equipped_weapon_id")]
    public Guid? EquippedWeaponId { get; set; }

    [Column("equipped_armor_id")]
    public Guid? EquippedArmorId { get; set; }

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
        
        /*
        If I set the ID of items as they enter the database,
        EFCore issues that as an update, which fails, because they don't exist.
        I want it to issue an insert, which requires the item ID to be empty.
        It's a pain to set the Equipped*Id properties after saving to the database,
        So I'll just require that players re-equip their stuff upon logging back in.

        EquippedWeaponId = gameModel.Weapon?.Id;
        EquippedArmorId = gameModel.Armor?.Id;
        */

        Items.Clear();
        AddEquipmentSlot(gameModel.Weapon);
        AddEquipmentSlot(gameModel.Armor);
        AddInventoryTab(gameModel.Inventory.Materials, i => 0);
        AddInventoryTab(gameModel.Inventory.Weapons, i => i.Level);
        AddInventoryTab(gameModel.Inventory.Armors, i => i.Level);
    }

    private void AddEquipmentSlot<T>(Equipment<T>? equipmentSlot)
    where T : IEquipmentStats
    {
        if (equipmentSlot != null)
        {
            Items.Add(DbPlayerCharacterItem.FromGameModel(
                this,
                equipmentSlot,
                equipmentSlot.Level,
                1
            ));
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
            var gameItems = dbItem.ToGameModel(itemFactory);
            foreach (var gameItem in gameItems)
            {
                result.Inventory.Add(gameItem);
            }
        }

        if (EquippedWeaponId != null)
        {
            result.EquipWeaponById(EquippedWeaponId.Value);
        }
        if (EquippedArmorId != null)
        {
            result.EquipArmorById(EquippedArmorId.Value);
        }

        return result;
    }
}
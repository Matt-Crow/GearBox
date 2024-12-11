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

    public DbEquippedItem? EquippedWeapon { get; set; }

    public DbEquippedItem? EquippedArmor { get; set; }

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

        EquippedWeapon = MakeEquipmentSlot(gameModel.Weapon);
        EquippedArmor = MakeEquipmentSlot(gameModel.Armor);     
        
        Items.Clear();
        AddInventoryTab(gameModel.Inventory.Materials, i => 0);
        AddInventoryTab(gameModel.Inventory.Weapons, i => i.Level);
        AddInventoryTab(gameModel.Inventory.Armors, i => i.Level);
    }

    private DbEquippedItem? MakeEquipmentSlot<T>(Equipment<T>? equipmentSlot)
    where T : IEquipmentStats
    {
        var dbModel = equipmentSlot == null
            ? null
            : new DbEquippedItem()
            {
                Name = equipmentSlot.Name,
                Level = equipmentSlot.Level
            };
        return dbModel;
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

        if (EquippedWeapon != null)
        {
            var gameItem = itemFactory.Make(EquippedWeapon.Name) ?? throw new Exception($"Invalid item name: {EquippedWeapon.Name}");
            result.Inventory.Add(gameItem);
            result.EquipWeaponById(gameItem.Id ?? throw new Exception("Weapon must have ID"));
        }
        if (EquippedArmor != null)
        {
            var gameItem = itemFactory.Make(EquippedArmor.Name) ?? throw new Exception($"Invalid item name: {EquippedArmor.Name}");
            result.Inventory.Add(gameItem);
            result.EquipArmorById(gameItem.Id ?? throw new Exception("Armor must have ID"));
        }

        return result;
    }
}
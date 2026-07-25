using GearBox.Core.Model.Items;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.ResourcePacks;

public class ItemStackResource<T>
where T : IItem
{
    public required string ItemName { get; set; }
    public required int Quantity { get; set; }

    public ItemStack<Material> ToItemStackOfMaterial(Factory<ItemUnion> items)
    {
        var item = items.Make(ItemName);
        
        // ensure the item with the given ItemName is a Material
        Material? material = null;
        item.Match(
            m => material = m,
            _ => {}
        );
        if (material == null)
        {
            throw new Exception($"Item with name '{ItemName}' exists, but is not a Material");
        }

        var result = new ItemStack<Material>(material, Quantity);
        return result;
    }
}
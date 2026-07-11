using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model.ResourcePacks;

public class ItemStackResource<T>
where T : IItem
{
    public required string ItemName { get; set; }
    public required int Quantity { get; set; }

    public ItemStack<Material> ToItemStackOfMaterial(IItemFactory items)
    {
        var result = new ItemStack<Material>(items.MakeMaterial(ItemName), Quantity);
        return result;
    }
}
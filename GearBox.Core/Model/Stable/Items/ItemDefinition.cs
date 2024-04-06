namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// Defines how to instantiate an item
/// </summary>
public readonly struct ItemDefinition
{
    public ItemDefinition(ItemType type, Func<ItemType, IItem> typeToItem)
    {
        Type = type;
        TypeToItem = typeToItem;
    }
    
    public ItemType Type { get; init; }
    public Func<ItemType, IItem> TypeToItem { get; init; }

    public IItem Create() => TypeToItem.Invoke(Type);
}
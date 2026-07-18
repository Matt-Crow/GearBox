using GearBox.Core.Model.Items;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.ResourcePacks;

public class LootOptionResource
{
    /// <summary>
    /// Either "item" or "gold"
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Only required when Type = "item"
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Must be 1 when Type = "item"
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// Only required when Type = "gold"
    /// </summary>
    public string Grade { get; set; } = "";


    public LootOption ToLootOption(Factory<ItemUnion> items)
    {
        if (Type == "item")
        {
            if (Name == "")
            {
                throw new Exception("Name is required when Type = \"item\"");
            }
            if (Quantity != 1)
            {
                throw new Exception("Quantity is ignored when Type = \"item\"");
            }
            if (Grade != "")
            {
                throw new Exception("Grade is ignored when Type = \"item\"");
            }
            var item = items.Make(Name);
            return new LootOption(item);
        }
        else if (Type == "gold")
        {
            if (Name != "")
            {
                throw new Exception("Name is ignored when Type = \"gold\"");
            }
            if (Quantity <= 0)
            {
                throw new Exception("Quantity must be at least 1 when Type = \"gold\"");
            }
            if (Grade == "")
            {
                throw new Exception("Grade is required when Type = \"gold\"");
            }
            var grade = GearBox.Core.Model.Items.Grade.GetGradeByName(Grade) ?? throw new Exception($"Invalid grade: '{Grade}'");
            return new LootOption(grade, new Gold(Quantity));
        }
        throw new Exception($"Invalid Type: '{Type}'");
    }
}
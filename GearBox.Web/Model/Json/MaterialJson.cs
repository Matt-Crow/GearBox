using GearBox.Core.Model.Items;

namespace GearBox.Web.Model.Json;

public class MaterialJson : IItemJson
{
    public required string Name { get; set; }
    public required string GradeName { get; set; }
    public required string Description { get; set; }

    public ItemUnion ToItem()
    {
        var grade = ItemJsonUtils.GetGradeByName(GradeName);
        var result = ItemUnion.Of(new Material(Name, grade, Description));
        return result;
    }
}
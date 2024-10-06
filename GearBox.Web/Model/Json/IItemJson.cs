using GearBox.Core.Model.Items;

namespace GearBox.Web.Model.Json;

public interface IItemJson
{
    ItemUnion ToItem();
}
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Json;

public class UiStateChangesJson
{
    public MaybeChangeJson<OpenShopJson?> OpenShop { get; set; }
}
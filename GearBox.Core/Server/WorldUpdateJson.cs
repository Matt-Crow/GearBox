using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Stable;

namespace GearBox.Core.Server;

/// <summary>
/// Notifies clients of changes to the world they're connected to
/// </summary>
public readonly struct WorldUpdateJson : IJson
{
    public WorldUpdateJson(
        DynamicWorldContentJson dynamicWorldContent,
        List<ChangeJson> changes
    )
    {
        DynamicWorldContent = dynamicWorldContent;
        Changes = changes;
    }
    
    public DynamicWorldContentJson DynamicWorldContent { get; init; }
    public List<ChangeJson> Changes { get; init; }
}
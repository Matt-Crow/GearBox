
namespace GearBox.Core.Model.Json.AreaUpdate;

public class PlayerStatSummaryJson : IChange
{
    public PlayerStatSummaryJson(List<string> lines)
    {
        Lines = lines;
    }
    
    public List<string> Lines { get; init; }

    public IEnumerable<object?> DynamicValues => Lines;
}
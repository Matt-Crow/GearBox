namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct PlayerStatSummaryJson
{
    public PlayerStatSummaryJson(List<string> lines)
    {
        Lines = lines;
    }
    
    public List<string> Lines { get; init; }
}
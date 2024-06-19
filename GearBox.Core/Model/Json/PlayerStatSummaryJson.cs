namespace GearBox.Core.Model.Json;

public readonly struct PlayerStatSummaryJson : IJson
{
    public PlayerStatSummaryJson(List<string> lines)
    {
        Lines = lines;
    }
    
    public List<string> Lines { get; init; }
}
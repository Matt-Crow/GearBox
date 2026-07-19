using GearBox.Core.Model.Areas;

namespace GearBox.Core.Model.ResourcePacks;

public class ExitResource
{
    public required string Type { get; set; }
    public required string DestinationName { get; set; }


    public IExit ToExit()
    {
        var result = Type.ToLower() switch
        {
            "left" => BorderExit.Left(DestinationName),
            "right" => BorderExit.Right(DestinationName),
            "top" => BorderExit.Top(DestinationName),
            "bottom" => BorderExit.Bottom(DestinationName),
            _ => throw new Exception($"Invalid exit type: '{Type}'")
        };
        return result;
    }
}
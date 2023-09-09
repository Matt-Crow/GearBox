namespace GearBox.Core.Model.Static;

public class StaticWorldContentJson : IJson
{
    public MapJson? Map { get; set; }
    public IEnumerable<IJson> GameObjects { get; set; } = new List<IJson>();
}
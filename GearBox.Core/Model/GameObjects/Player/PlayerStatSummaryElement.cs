namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerStatSummaryElement
{
    private readonly string _label;
    private readonly Func<PlayerCharacter, string> _format;
    private readonly Func<PlayerCharacter, IEnumerable<object?>> _dependencies;

    
    public PlayerStatSummaryElement(string label, Func<PlayerCharacter, string> format, Func<PlayerCharacter, IEnumerable<object?>> dependencies)
    {
        _label = label;
        _format = format;
        _dependencies = dependencies;
    }


    public IEnumerable<object?> DynamicValues(PlayerCharacter player)
    {
        return _dependencies.Invoke(player);
    }

    public string ToString(PlayerCharacter player)
    {
        return $"{_label}: {_format(player)}";
    }
}
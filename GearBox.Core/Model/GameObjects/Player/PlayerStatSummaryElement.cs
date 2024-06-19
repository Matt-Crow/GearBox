namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerStatSummaryElement
{
    private readonly Func<PlayerCharacter, string> _format;
    private readonly Func<PlayerCharacter, IEnumerable<object?>> _dependencies;

    
    public PlayerStatSummaryElement(Func<PlayerCharacter, string> format, Func<PlayerCharacter, IEnumerable<object?>> dependencies)
    {
        _format = format;
        _dependencies = dependencies;
    }


    public IEnumerable<object?> DynamicValues(PlayerCharacter player)
    {
        return _dependencies.Invoke(player);
    }

    public string ToString(PlayerCharacter player) => _format(player);
}
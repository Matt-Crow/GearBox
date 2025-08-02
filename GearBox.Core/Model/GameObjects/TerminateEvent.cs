namespace GearBox.Core.Model.GameObjects;

public class TerminateEvent
{
    public TerminateEvent(IGameObject terminated)
    {
        Terminated = terminated;
    }

    public IGameObject Terminated { get; init; }
}
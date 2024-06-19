namespace GearBox.Core.Model.GameObjects;

public class TerminateEventArgs : EventArgs
{
    public TerminateEventArgs(IGameObject terminated)
    {
        Terminated = terminated;
    }

    public IGameObject Terminated { get; init; }
}
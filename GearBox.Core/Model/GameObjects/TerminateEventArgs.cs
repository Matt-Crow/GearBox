namespace GearBox.Core.Model.GameObjects;

public class TerminateEventArgs : EventArgs
{
    public TerminateEventArgs(IDynamicGameObject terminated)
    {
        Terminated = terminated;
    }

    public IDynamicGameObject Terminated { get; init; }
}
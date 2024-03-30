namespace GearBox.Core.Model.Dynamic;

public class TerminateEventArgs : EventArgs
{
    public TerminateEventArgs(IDynamicGameObject terminated)
    {
        Terminated = terminated;
    }

    public IDynamicGameObject Terminated { get; init; }
}
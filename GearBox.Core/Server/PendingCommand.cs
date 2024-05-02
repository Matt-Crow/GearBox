using GearBox.Core.Controls;

namespace GearBox.Core.Server;

/// <summary>
/// Wraps an IControlCommand which has yet to execute
/// </summary>
public class PendingCommand
{
    public PendingCommand(string connectionId, IControlCommand command)
    {
        ConnectionId = connectionId;
        Command = command;
    }
    
    public string ConnectionId { get; init; }
    public IControlCommand Command { get; init; }
}
using GearBox.Core.Controls;
using GearBox.Core.Server;
using Microsoft.AspNetCore.SignalR;

namespace GearBox.Web.Infrastructure;

// Hubs are transient: don't try storing data on them
public class WorldHub : Hub
{
    private readonly WorldServer _server;

    public WorldHub(WorldServer server)
    {
        _server = server;
    }

    public override async Task OnConnectedAsync()
    {
        var id = Context.ConnectionId;
        // new player
        await _server.AddConnection(id, new WorldHubConnection(Clients.Caller));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _server.RemoveConnection(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Called by Javascript, calls Javascript on all clients.
    /// </summary>
    /// <param name="message">the message sent by Javascipt</param>
    /// <returns>A task which resolves once Javascript is invoked on all clients</returns>
    public async Task Send(string message)
    {
        //var sender = Context.User;
        if (message == "r")
        {
            await StartMovingRight();
        }
        else
        {
            await StopMovingRight();
        }
    }

    /*
        Not sure how much I like this idea, 
        as while it is very few lines of code,
        it is a lot of little methods.

        Maybe I'll come back to this once I've settled on serialization methods.
    */
    public Task StartMovingUp() => Receive(StartMoving.UP);
    public Task StartMovingDown() => Receive(StartMoving.DOWN);
    public Task StartMovingLeft() => Receive(StartMoving.LEFT);
    public Task StartMovingRight() => Receive(StartMoving.RIGHT);

    public Task StopMovingUp() => Receive(StopMoving.UP);
    public Task StopMovingDown() => Receive(StopMoving.DOWN);
    public Task StopMovingLeft() => Receive(StopMoving.LEFT);
    public Task StopMovingRight() => Receive(StopMoving.RIGHT);

    private Task Receive(IControlCommand command)
    {
        var id = Context.ConnectionId;
        var controller = _server.GetControlsById(id) ?? throw new Exception($"Invalid id: \"{id}\"");
        controller.Receive(command);
        return Task.CompletedTask;
    }
}
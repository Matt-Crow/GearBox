using GearBox.Core.Controls;
using GearBox.Core.Model.Units;
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

    public Task Equip(Guid id) => Receive(new Equip(id));
    public Task StartMovingUp() => Receive(StartMoving.UP);
    public Task StartMovingDown() => Receive(StartMoving.DOWN);
    public Task StartMovingLeft() => Receive(StartMoving.LEFT);
    public Task StartMovingRight() => Receive(StartMoving.RIGHT);

    public Task StopMovingUp() => Receive(StopMoving.UP);
    public Task StopMovingDown() => Receive(StopMoving.DOWN);
    public Task StopMovingLeft() => Receive(StopMoving.LEFT);
    public Task StopMovingRight() => Receive(StopMoving.RIGHT);
    public Task Respawn() => Receive(new Respawn());

    /// <summary>
    /// Note the parameter is the bearing in degrees, so 0 means up, 90 means to the right, etc.
    /// </summary>
    public Task UseBasicAttack(int bearingInDegrees) => Receive(new UseBasicAttackCommand(Direction.FromBearingDegrees(bearingInDegrees)));

    private Task Receive(IControlCommand command)
    {
        var id = Context.ConnectionId;
        _server.EnqueueCommand(id, command);
        return Task.CompletedTask;
    }
}
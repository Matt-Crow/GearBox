using GearBox.Core.Controls;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;
using GearBox.Core.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GearBox.Web.Infrastructure;

// Hubs are transient: don't try storing data on them
[Authorize]
public class GameHub : Hub
{
    private readonly GameServer _server;

    public GameHub(GameServer server)
    {
        _server = server;
    }

    public override async Task OnConnectedAsync()
    {
        var id = Context.ConnectionId;
        var userId = Context.UserIdentifier ?? throw new Exception("User must be authenticated");
        var userName = Context.User?.Identity?.Name ?? throw new Exception("User must be authenticated");
        var user = new ConnectingUser(userId, userName);
        await _server.AddConnection(id, user, new GameHubConnection(Clients.Caller));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _server.RemoveConnection(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public Task EquipWeapon(Guid id) => Receive(new EquipWeapon(id));
    public Task EquipArmor(Guid id) => Receive(new EquipArmor(id));
    public Task StartMovingUp() => Receive(StartMoving.UP);
    public Task StartMovingDown() => Receive(StartMoving.DOWN);
    public Task StartMovingLeft() => Receive(StartMoving.LEFT);
    public Task StartMovingRight() => Receive(StartMoving.RIGHT);

    public Task StopMovingUp() => Receive(StopMoving.UP);
    public Task StopMovingDown() => Receive(StopMoving.DOWN);
    public Task StopMovingLeft() => Receive(StopMoving.LEFT);
    public Task StopMovingRight() => Receive(StopMoving.RIGHT);
    public Task Respawn() => Receive(new Respawn());
    public Task Craft(Guid recipeId) => Receive(new Craft(recipeId));
    public Task OpenShop() => Receive(new OpenShop());
    public Task CloseShop() => Receive(new CloseShop());
    public Task ShopBuy(Guid shopId, Guid? itemId, string? itemName) => Receive(new ShopBuy(shopId, new ItemSpecifier(itemId, itemName)));
    public Task ShopSell(Guid shopId, Guid? itemId, string? itemName) => Receive(new ShopSell(shopId, new ItemSpecifier(itemId, itemName)));

    /// <summary>
    /// Note the parameter is the bearing in degrees, so 0 means up, 90 means to the right, etc.
    /// </summary>
    public Task UseBasicAttack(int bearingInDegrees) => Receive(new UseBasicAttack(Direction.FromBearingDegrees(bearingInDegrees)));

    public Task UseActive(int number, int bearingInDegrees) => Receive(new UseActive(number, Direction.FromBearingDegrees(bearingInDegrees)));

    private Task Receive(IControlCommand command)
    {
        var id = Context.ConnectionId;
        _server.EnqueueCommand(id, command);
        return Task.CompletedTask;
    }
}
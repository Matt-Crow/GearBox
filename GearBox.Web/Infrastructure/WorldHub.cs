using System.Text.Json;
using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Server;
using Microsoft.AspNetCore.SignalR;

namespace GearBox.Web.Infrastructure;

public class WorldHub : Hub
{
    private readonly WorldServer _server;

    public WorldHub(WorldServer server)
    {
        _server = server;
    }

    public override async Task OnConnectedAsync()
    {
        // new player
        await _server.AddConnection(new WorldHubConnection(Clients.Caller));
    }

    /// <summary>
    /// Called by Javascript, calls Javascript on all clients.
    /// </summary>
    /// <param name="message">the message sent by Javascipt</param>
    /// <returns>A task which resolves once Javascript is invoked on all clients</returns>
    public async Task Send(string message)
    {
        //var sender = Context.User;
        await Clients.All.SendAsync("receive", $"{{\"message:\": \"{message}\"}}");
    }
}
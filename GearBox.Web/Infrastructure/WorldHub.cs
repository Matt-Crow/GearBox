using Microsoft.AspNetCore.SignalR;

namespace GearBox.Web.Infrastructure;

public class WorldHub : Hub
{
    /// <summary>
    /// Called by Javascript, calls Javascript on all clients.
    /// </summary>
    /// <param name="message">the message sent by Javascipt</param>
    /// <returns>A task which resolves once Javascript is invoked on all clients</returns>
    public async Task Send(string message)
    {
        //var sender = Context.User;
        await Clients.All.SendAsync("receive", $"Someone said \"{message}\"");
    }
}
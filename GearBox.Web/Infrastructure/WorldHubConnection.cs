using System.Text.Json;
using System.Text.Json.Serialization;
using GearBox.Core.Model;
using GearBox.Core.Server;
using Microsoft.AspNetCore.SignalR;

namespace GearBox.Web.Infrastructure;

public class WorldHubConnection : IConnection
{
    private readonly ISingleClientProxy _player;

    public WorldHubConnection(ISingleClientProxy player)
    {
        _player = player;
    }

    public async Task Send<T>(Message<T> message) where T : IJson
    {
        var sendMe = JsonSerializer.Serialize(message.Body, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { 
                new JsonStringEnumConverter() 
            }
        });
        await _player.SendAsync("receive", sendMe);
    }

    public Task<Message<T>> Receive<T>() where T : IJson
    {
        throw new NotImplementedException();
    }
}
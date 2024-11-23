using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Server;

/// <summary>
/// A connection to the server which is controlling a player.
/// </summary>
public class PlayerConnection
{
    private readonly string _aspNetUserId;
    private readonly IConnection _connection;
    private readonly PlayerCharacter _player;
    private UiState? _uiState; // start with null UI state so it detects changes

    public PlayerConnection(string aspNetUserId, IConnection connection, PlayerCharacter player)
    {
        _aspNetUserId = aspNetUserId;
        _connection = connection;
        _player = player;
    }

    /// <summary>
    /// The player controlled by this connection.
    /// </summary>
    public PlayerCharacter Player => _player;

    /// <summary>
    /// Notifies the connection that it has connected to the given game.
    /// </summary>
    public async Task HandleConnect(IGame game)
    {
        var area = game.GetDefaultArea();
        var spawnLocation = area.GetRandomFloorTile();
        _player.Coordinates = spawnLocation.CenteredOnTile();
        area.SpawnPlayer(_player);

        var gameInitJson = game.GetGameInitJsonFor(_player);
        await _connection.Send("GameInit", gameInitJson);

        await SendAreaUpdate();
    }

    /// <summary>
    /// Notifies the connection that it has disconnected from the server.
    /// </summary>
    public async Task HandleDisconnect(IPlayerCharacterRepository repository)
    {
        await repository.SavePlayerCharacterAsync(_player, _aspNetUserId);
        _player.CurrentArea?.RemovePlayer(_player);
    }

    /// <summary>
    /// Sends a message to the client, containing the updates to their area.
    /// </summary>
    public Task SendAreaUpdate()
    {
        var area = _player.CurrentArea ?? _player.LastArea; // last area in case they died
        if (area == null)
        {
            return Task.CompletedTask;
        }
        var json = area.GetAreaUpdateJsonFor(_player);
        var newUiState = new UiState(_player);
        var uiStateChanges = UiState.GetChanges(_uiState, newUiState);
        _uiState = newUiState; // must come after computing changes
        json.UiStateChanges = uiStateChanges;
        return _connection.Send("AreaUpdate", json);
    }
}

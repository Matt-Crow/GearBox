namespace GearBox.Core.Server;

/// <summary>
/// An authenticated user who is connecting to the server
/// </summary>
public class ConnectingUser
{
    public ConnectingUser(string id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public string Id { get; init; }
    public string Name { get; init; }
}
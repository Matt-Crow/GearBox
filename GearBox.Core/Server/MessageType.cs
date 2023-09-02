namespace GearBox.Core.Server;

// not sure how much I like using enums...
public enum MessageType
{
    /// <summary>
    /// designates this message contains a new world
    /// </summary>
    WorldInit,
    
    /// <summary>
    /// designates this message contains an update to a world
    /// </summary>
    WorldUpdate
}
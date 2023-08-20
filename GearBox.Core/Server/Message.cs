namespace GearBox.Core.Server;

using GearBox.Core.Model;

public class Message
{
    public Message(MessageType type, ISerializable body)
    {
        Type = type;
        Body = body;
    }

    public MessageType Type { get; init; }
    public ISerializable Body { get; init; }
}
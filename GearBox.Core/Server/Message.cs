namespace GearBox.Core.Server;

using GearBox.Core.Model;

// must be generic to serialize Body as JSON: 
// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/polymorphism?pivots=dotnet-7-0
public readonly struct Message<T>
where T : IJson
{
    public Message(MessageType type, T body)
    {
        Type = type;
        Body = body;
    }

    public MessageType Type { get; init; }
    public T Body { get; init; }
}
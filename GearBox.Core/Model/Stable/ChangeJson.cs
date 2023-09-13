namespace GearBox.Core.Model.Stable;

todo need to add deserializers to client
public readonly struct ChangeJson : IJson
{
    public ChangeJson(ChangeType changeType, string bodyType, string body)
    {
        ChangeType = changeType;
        BodyType = bodyType;
        Body = body;
    }

    public ChangeType ChangeType { get; init; }
    public string BodyType { get; init; }
    public string Body { get; init; }
}
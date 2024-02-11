using GearBox.Core.Model.Stable;

namespace GearBox.Core.Model.Json;

public readonly struct ChangeJson : IJson
{
    public ChangeJson(string bodyType, string body, bool isDelete)
    {
        BodyType = bodyType;
        Body = body;
        IsDelete = isDelete;
    }

    public string BodyType { get; init; }
    public string Body { get; init; }
    public bool IsDelete { get; init; }
}
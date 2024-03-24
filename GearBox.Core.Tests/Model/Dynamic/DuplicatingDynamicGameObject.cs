using System.Text.Json;
using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Tests.Model.Dynamic;

public class DuplicatingDynamicGameObject : IDynamicGameObject
{
    private readonly DynamicWorldContent _content;

    public DuplicatingDynamicGameObject(DynamicWorldContent content) => _content = content;

    public string Type => "";
    public BodyBehavior? Body => null;
    public bool IsTerminated => false;

    public void Update() => _content.AddDynamicObject(new DuplicatingDynamicGameObject(_content));
    
    public string Serialize(JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
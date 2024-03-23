using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Tests.Model.Dynamic;

public class DuplicatingDynamicGameObject : IDynamicGameObject
{
    private readonly DynamicWorldContent _content;

    public DuplicatingDynamicGameObject(DynamicWorldContent content) => _content = content;

    public BodyBehavior? Body => null;
    public bool IsTerminated => false;

    public IDynamicGameObjectJson ToJson() => throw new NotImplementedException();

    public void Update() => _content.AddDynamicObject(new DuplicatingDynamicGameObject(_content));
}
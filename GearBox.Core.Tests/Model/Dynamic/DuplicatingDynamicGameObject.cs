using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Tests.Model.Dynamic;

public class DuplicatingDynamicGameObject : IDynamicGameObject
{
    private readonly DynamicWorldContent _content;

    public DuplicatingDynamicGameObject(DynamicWorldContent content) => _content = content;

    public Serializer? Serializer => null;
    public BodyBehavior? Body => null;
    public bool IsTerminated => false;

    public void Update() => _content.AddDynamicObject(new DuplicatingDynamicGameObject(_content));
}
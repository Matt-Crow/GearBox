using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Tests.Model.GameObjects;

public class DuplicatingDynamicGameObject : IGameObject
{
    private readonly DynamicWorldContent _content;

    public DuplicatingDynamicGameObject(DynamicWorldContent content) => _content = content;

    public Serializer? Serializer => null;
    public BodyBehavior? Body => null;
    public TerminateBehavior? Termination => null;

    public void Update() => _content.AddDynamicObject(new DuplicatingDynamicGameObject(_content));
}
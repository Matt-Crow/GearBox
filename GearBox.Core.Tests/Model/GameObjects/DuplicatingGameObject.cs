using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Tests.Model.GameObjects;

public class DuplicatingGameObject : IGameObject
{
    private readonly GameObjectCollection<DuplicatingGameObject> _content;

    public DuplicatingGameObject(GameObjectCollection<DuplicatingGameObject> content) => _content = content;

    public Serializer? Serializer => null;
    public BodyBehavior? Body => null;
    public TerminateBehavior? Termination => null;

    public void Update() => _content.AddGameObject(new DuplicatingGameObject(_content));
}
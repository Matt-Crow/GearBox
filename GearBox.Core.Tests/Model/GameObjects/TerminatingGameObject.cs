using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Tests.Model.GameObjects;

public class TerminatingGameObject : IGameObject
{
    public TerminatingGameObject()
    {
        Termination = new(this, () => IsTerminated);
    }

    public Serializer? Serializer => null;
    public BodyBehavior? Body => null;
    public TerminateBehavior? Termination { get; init; }
    public bool IsTerminated { get; set; }

    public void Update()
    {
        
    }
}

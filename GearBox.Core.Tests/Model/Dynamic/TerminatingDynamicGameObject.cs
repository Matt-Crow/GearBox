using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Tests.Model.Dynamic;

public class TerminatingDynamicGameObject : IDynamicGameObject
{
    public TerminatingDynamicGameObject()
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

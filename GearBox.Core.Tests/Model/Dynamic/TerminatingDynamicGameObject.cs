using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Tests.Model.Dynamic;

public class TerminatingDynamicGameObject : IDynamicGameObject
{
    public Serializer? Serializer => null;
    public BodyBehavior? Body => null;

    public bool IsTerminated { get; set; }

    public void Update()
    {
        
    }
}

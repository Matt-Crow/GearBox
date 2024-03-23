using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Tests.Model.Dynamic;

public class TerminatingDynamicGameObject : IDynamicGameObject
{
    public BodyBehavior? Body => null;

    public bool IsTerminated { get; set; }

    public IDynamicGameObjectJson ToJson()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        
    }
}

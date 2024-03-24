using System.Text.Json;
using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Tests.Model.Dynamic;

public class TerminatingDynamicGameObject : IDynamicGameObject
{
    public string Type => "";
    public BodyBehavior? Body => null;

    public bool IsTerminated { get; set; }

    public void Update()
    {
        
    }

    public string Serialize(JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

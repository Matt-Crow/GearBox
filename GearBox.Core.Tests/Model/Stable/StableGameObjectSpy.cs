
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

public class StableGameObjectSpy : IStableGameObject
{
    public int Foo { get; set; }
    public Serializer? Serializer => null;
    public IEnumerable<object?> DynamicValues { get => new List<object>() {Foo}; }

    public void Update()
    {
        
    }
}
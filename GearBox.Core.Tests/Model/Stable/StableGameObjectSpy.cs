
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

public class StableGameObjectSpy : IStableGameObject
{
    public int Foo { get; set; }

    public string Type { get => "spy"; }
    public IEnumerable<object?> DynamicValues { get => new List<object>() {Foo}; }

    public void Update()
    {
        
    }

    public string Serialize(JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(this, options);
    }
}
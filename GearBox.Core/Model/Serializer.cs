using GearBox.Core.Model.Json.AreaUpdate.GameObjects;
using System.Text.Json;

namespace GearBox.Core.Model;

/// <summary>
/// Serialization behavior used by IGameObject
/// </summary>
public class Serializer
{
    private static readonly JsonSerializerOptions JSON_OPTIONS = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    
    private readonly string _type;
    private readonly Func<SerializationOptions, string> _mapper;

    public Serializer(string type, Func<SerializationOptions, string> mapper)
    {
        _type = type;
        _mapper = mapper;
    }

    public GameObjectJson Serialize()
    {
        var options = new SerializationOptions();
        var result = new GameObjectJson(_type, _mapper.Invoke(options));
        return result;
    }
}
using System.Text.Json;

namespace GearBox.Core.Model;

public interface IGameObject
{
    /// <summary>
    /// What type this will be listed as when serialized
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Called each game tick.
    /// Subclasses should not call this method.
    /// </summary>
    void Update();

    /// <summary>
    /// Serializes this object so it can be sent to the front-end.
    /// Note that Type is automatically provided to the front-end.
    /// </summary>
    string Serialize(JsonSerializerOptions options);
}
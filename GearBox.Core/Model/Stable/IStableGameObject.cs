using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// A stable game object is one which changes infrequently, and thus only needs 
/// to be synced with the clients occasionally.
/// </summary>
public interface IStableGameObject : IDynamic
{
    /// <summary>
    /// What this will be listed as when serialized
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Called each game tick.
    /// Subclasses should not call this method.
    /// </summary>
    public void Update();

    public string Serialize(JsonSerializerOptions options);
}
namespace GearBox.Core.Model.Json;

/// <summary>
/// Used by UiState to decide whether or not it should send a large object to a client.
/// </summary>
public interface IChange
{
    /// <summary>
    /// Used to compare two objects to see if they have changed.
    /// All elements of this collection should be value types or null, 
    /// as reference types with the same values are not considered equal due to referential equality.
    /// 
    /// If you encounter bugs where the object is incorrectly flagged as "changed" each tick,
    /// it is most likely because this collection contains a reference type.
    /// </summary>
    public IEnumerable<object?> DynamicValues { get; }
}
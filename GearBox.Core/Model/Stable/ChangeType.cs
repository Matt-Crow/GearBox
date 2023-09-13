namespace GearBox.Core.Model.Stable;

/// <summary>
/// Differentiates between different types of changes
/// </summary>
public enum ChangeType
{
    /// <summary>
    /// something new was created
    /// </summary>
    Create,

    /// <summary>
    /// something existing has been modified
    /// </summary>
    Update,

    /// <summary>
    /// something existing no longer exists
    /// </summary>
    Delete
}
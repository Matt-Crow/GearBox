namespace GearBox.Core.Model.Stable;

public class DynamicHasher
{
    /// <summary>
    /// Hashes the dynamic values of the given object.
    /// Not to be confused with the object's hashcode, which is expected to remain constant.
    /// </summary>
    public int Hash(IDynamic obj)
    {
        var result = new HashCode();
        foreach (var field in obj.DynamicValues)
        {
            result.Add(field);
        }
        var hashCode = result.ToHashCode();
        return hashCode;
    }
}
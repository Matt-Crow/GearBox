namespace GearBox.Core.Model.Json;

/// <summary>
/// Designates that a class can be serialized and sent as a message
/// </summary>
public interface ISerializable<T> where T: IJson
{
    /// <summary>
    /// returns a json-serializable version of this object
    /// </summary>
    /// <returns>this object, but as a json-serializable class</returns>
    public T ToJson();
}
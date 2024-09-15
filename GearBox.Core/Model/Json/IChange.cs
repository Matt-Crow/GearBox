namespace GearBox.Core.Model.Json;

public interface IChange
{
    public IEnumerable<object?> DynamicValues { get; }
}
namespace GearBox.Core.Utils;

public static class ListExtensions
{
    // static extension methods don't work
    public static List<T> Of<T>(T element) => [element];
}
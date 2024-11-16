using Npgsql;

namespace GearBox.Web.Database;

public static class SearchPathHelper
{
    /// <summary>
    /// Returns the search path in the given connection string, or "public" if none is provided
    /// </summary>
    public static string GetSearchPathByConnectionString(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var searchPath = builder.SearchPath ?? "public";
        return searchPath;
    }
}
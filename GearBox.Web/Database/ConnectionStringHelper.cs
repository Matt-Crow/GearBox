using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace GearBox.Web.Database;

public static class ConnectionStringHelper
{
    /// <summary>
    /// Uses the Postgres database connection string provided by config,
    /// or an in-memory database if no such connection string exists.
    /// </summary>
    /// <param name="config">the configuration to read from</param>
    /// <param name="name">the name of the connection string to check for</param>
    /// <returns>something to feed to AddDbContext or AddDbContextFactory</returns>
    public static Action<DbContextOptionsBuilder> UsePostgresOrInMemory(ConfigurationManager config, string name)
    {
        var connectionString = config.GetConnectionString(name);
        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine($"No connection string provided for '{name}' - defaulting to in-memory database");
            return options => options.UseInMemoryDatabase(name);
        }
        
        return options => options.UseNpgsql(connectionString, UseMigrationHistoryTableInSearchPath(connectionString));
    }

    private static Action<NpgsqlDbContextOptionsBuilder> UseMigrationHistoryTableInSearchPath(string connectionString)
    {
        /*
            By default, EFCore only checks the "public" schema for applied migrations, even if a SearchPath is provided in the connection string.
            This causes all migrations to be marked as "pending", as I apply those migrations to another schema.
            To resolve this issue, I'm overwriting where it searches for the applied migrations.
            https://github.com/npgsql/efcore.pg/issues/3180
        */
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var searchPath = builder.SearchPath ?? "public";
        return options => options.MigrationsHistoryTable("__EFMigrationsHistory", searchPath);
    }
}
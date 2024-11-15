# GearBox
A C# MMORPG-like game.

## Running Tests
`dotnet watch --project GearBox.Core.Tests test`

## Controls

- **WASD** to move 
- **Q** to use basic attack
- **E** to toggle inventory or open shop
- **1-9** to use active abilities (if any)

## Creating Migrations
`dotnet ef migrations add <NAME> -c GearBox.Web.Database.GameDbContext -o Migrations/Game`
`dotnet ef migrations add <NAME> -c GearBox.Web.Database.IdentityDbContext -o Migrations/Identity`

## Setting up the environment
You can skip all this setup by not providing a connection string,
at which point the program uses an in-memory database.

### 1. Install a Database
This project uses PostgeSQL v17, which you can download from [PostgreSQL.org](https://www.postgresql.org/download/).
Since this project queries the database through Entity Framework Core,
so you might be able to use any database supported by it.
Once you have a PostgreSQL database, go to the next step.

### 2. Initialize the Database
Before you can run the Entity Framework Core migrations,
you'll need to set up the database for it.
```
CREATE DATABASE gearbox;
\c gearbox
CREATE SCHEMA game;
CREATE SCHEMA identity;
CREATE USER efcore WITH PASSWORD '<PASSWORD>';
GRANT ALL PRIVILEGES ON SCHEMA game TO efcore;
GRANT ALL PRIVILEGES ON SCHEMA identity TO efcore;
```

### 3. Set the Connection String
Set the connection strings in `/GearBox.Web/appsettings.Development.json`:
```
{
    "ConnectionStrings": {
        "game": "host=localhost;port=5432;database=gearbox;SearchPath=game;username=efcore;password=<PASSWORD>",
        "identity": "host=localhost;port=5432;database=gearbox;SearchPath=identity;username=efcore;password=<PASSWORD>"
    }
}
```

### 4. Run Migrations
If you haven't already installed `dotnet-ef`, run `dotnet tool install --global dotnet-ef`.
Once you have that installed, `cd` into `/GearBox.Web`, then run 
`dotnet ef database update -c GearBox.Web.Database.IdentityDbContext`
and
`dotnet ef database update -c GearBox.Web.Database.GameDbContext`.

### 5. Revoke Permissions
Once you've run migrations, it is a good practice to revoke excess permissions from service account like so:
```
REVOKE CREATE ON SCHEMA game FROM efcore;
REVOKE CREATE ON SCHEMA identity FROM efcore;
```
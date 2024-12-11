# GearBox
A C# ARPG-like game.

## Running Tests
`dotnet watch --project GearBox.Core.Tests test`

## Controls

- **WASD** to move 
- **Q** to use basic attack
- **E** to toggle inventory or open shop
- **1-9** to use active abilities (if any)

## Creating Migrations
`dotnet ef migrations add <NAME>`

## Setting up the environment

### Configuring Emails
I use GMail to send application emails, but you can log emails to the console instead when testing.

#### Using the console sender
This should only be used for testing, but you can use the console sender by adding this block to your appsettings:
```
"Email": {
    "SendEmails": false
}
```

#### Using GMail
In your appsetting file, add this block:
```
"Email": {
    "SendEmails": true,
    "SenderEmailAddress": <any email address>
}
```

You'll also need to set up the GMail API, like so:

1. On https://console.cloud.google.com/, create a new project.
2. Enable the GMail API for the project.
3. Create a new desktop OAuth 2.0 Client ID
4. Download the OAuth client secret, then store it as `/secrets/client_secret.json`

### Database setup
You can skip all this setup by not providing a connection string,
at which point the program uses an in-memory database.

#### 1. Install a Database
This project uses PostgeSQL v17, which you can download from [PostgreSQL.org](https://www.postgresql.org/download/).
Since this project queries the database through Entity Framework Core,
so you might be able to use any database supported by it.
Once you have a PostgreSQL database, go to the next step.

#### 2. Initialize the Database
Before you can run the Entity Framework Core migrations,
you'll need to set up the database for it.
```
CREATE DATABASE gearbox;
\c gearbox
CREATE SCHEMA gb;
CREATE USER efcore WITH PASSWORD '<PASSWORD>';
GRANT ALL PRIVILEGES ON SCHEMA gb TO efcore;
```

#### 3. Set the Connection String
Set the connection strings in `/GearBox.Web/appsettings.Development.json`:
```
{
    "ConnectionStrings": {
        "GearBoxDbContext": "host=localhost;port=5432;database=gearbox;SearchPath=gb;username=efcore;password=<PASSWORD>"
    }
}
```

#### 4. Run Migrations
If you haven't already installed `dotnet-ef`, run `dotnet tool install --global dotnet-ef`.
Once you have that installed, `cd` into `/GearBox.Web`, then run 
`dotnet ef database update`.

#### 5. Revoke Permissions
Once you've run migrations, it is a good practice to revoke excess permissions from service account like so:
`REVOKE CREATE ON SCHEMA gb FROM efcore;`

## Notes
The identity pages were scaffolded by running 
```
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet aspnet-codegenerator identity -dc GearBox.Web.Database.IdentityDbContext -f
```
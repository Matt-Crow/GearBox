using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.Abilities.Actives.Impl;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;
using GearBox.Web.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/*
    Need to load some of the game resources in a specific order:
    1. actives
    2. items, which use actives
    3. crafting recipes, which use items
    4. enemies, which use items
    5. areas, which use items and enemies
*/

// need to grab configuration before most other things
var webAppBuilder = WebApplication.CreateBuilder(args);

var gearboxConfig = new GearBoxConfig();
webAppBuilder.Configuration
    .GetSection("GearBox")
    .Bind(gearboxConfig);
var gameBuilder = new GameBuilder(gearboxConfig);

// configure actives before items
gameBuilder.Actives
    .Add(new Cleave())
    .Add(new Firebolt())
    ;

// configure items before crafting recipes and enemies
var resourceLoader = new GameResourceLoader(gameBuilder.Actives);
var itemResources = await resourceLoader.LoadAllItems();
foreach (var item in itemResources)
{
    gameBuilder.Items.Add(item);
}

// configure crafting recipes after items
gameBuilder
    .AddCraftingRecipe(recipe => recipe
        .And("Bronze", 25)
        .Makes("Bronze Khopesh")
    )
    .AddCraftingRecipe(recipe => recipe
        .And("Bronze", 25)
        .Makes("Bronze Armor")
    );

// configure enemies after items
gameBuilder.Enemies
    .Add("Snake", Color.LIGHT_GREEN, loot => loot
        .AddItem("Fang")
        .AddItem("Bronze")
        .Add(Grade.COMMON, new Gold(5))
    )
    .Add("Scorpion", Color.BLACK, loot => loot
        .AddItem("Fighter Initiate's Armor")
        .AddItem("Bronze")
        .Add(Grade.UNCOMMON, new Gold(10))
    )
    .Add("Jackal", Color.TAN, loot => loot
        .AddItem("Fang")
        .Add(Grade.RARE, new Gold(25))
    );

// we have all the game data, now make areas in that game
var bazaarMap = await resourceLoader.LoadMapByName("bazaar");
var desertMap = await resourceLoader.LoadMapByName("desert");
var canyonMap = await resourceLoader.LoadMapByName("canyon");
gameBuilder
    .WithArea("desert", 1, area => area
        .AddLoot(loot => loot
            .AddItem("Stone")
            .AddItem("Bronze")
            .Add(Grade.COMMON, new Gold(5))
            .Add(Grade.UNCOMMON, new Gold(10))
        )
        .AddEnemies(enemies => enemies
            .Add("Snake")
            .Add("Scorpion")
        )
        .WithMap(desertMap)
        .WithExit(BorderExit.Bottom("bazaar"))
        .WithExit(BorderExit.Right("canyon"))
    )
    .WithArea("bazaar", 1, area => area
        .WithMap(bazaarMap)
        .AddShop("Starter Weapon Shop", Coordinates.FromTiles(2, 7), Color.BLUE, shop => shop
            .AddItem("Training Sword")
            .AddItem("Training Bow")
            .AddItem("Training Staff")
            .AddItem("Fighter Initiate's Armor")
            .AddItem("Archer Initiate's Armor")
            .AddItem("Mage Initiate's Armor")
        )
        .WithExit(BorderExit.Top("desert"))
    )
    .WithArea("canyon", 2, area => area
        .AddLoot(loot => loot
            .AddItem("Bronze")
            .AddItem("Silver")
            .Add(Grade.RARE, new Gold(25))
        )
        .AddEnemies(enemies => enemies
            .Add("Snake")
            .Add("Jackal")
        )
        .WithMap(canyonMap)
        .WithExit(BorderExit.Left("desert"))
    );

// done defining - time to build
var game = gameBuilder.Build();

// Add services to the container.
var connString = webAppBuilder.Configuration.GetConnectionString("GearBox");
if (string.IsNullOrEmpty(connString))
{
    Console.WriteLine("No connection string provided - defaulting to in-memory database");
    webAppBuilder.Services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("GearBox"));   
}
else
{
    webAppBuilder.Services.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(connString));
}
webAppBuilder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = !true) // change back to true in #119
    .AddEntityFrameworkStores<IdentityDbContext>();
webAppBuilder.Services.AddRazorPages();
webAppBuilder.Services.AddSignalR();
webAppBuilder.Services.AddSingleton(new GameServer(game));

var app = webAppBuilder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// need to keep the middleware pipeline in order: don't merge into the other if statement!
if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles(new StaticFileOptions()
    {
        OnPrepareResponse = (ctx) => ctx.Context.Response.Headers.Append("Cache-Control", "no-store")
    });
}
else
{
    app.UseStaticFiles();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapHub<GameHub>("/area-hub");

app.Run();

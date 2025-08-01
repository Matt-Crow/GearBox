using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.Abilities.Actives.Impl;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;
using GearBox.Web.Database;
using GearBox.Web.Email;
using GearBox.Core.Model.GameObjects.Player;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using GearBox.Core.Model.Areas;
using GearBox.Core.Utils;

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
var rng = new RandomNumberGenerator();
var gameBuilder = new GameBuilder(gearboxConfig, rng);

// configure actives before items
gameBuilder.Actives
    .Add(new Cleave())
    .Add(new Firebolt())
    ;

// configure items before crafting recipes and enemies
var resourceLoader = new GameResourceLoader(gameBuilder.Actives, rng);
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
var config = webAppBuilder.Configuration;
webAppBuilder.Services.AddDbContextFactory<GearBoxDbContext>(ConnectionStringHelper.UsePostgresOrInMemory(config, "GearBoxDbContext"));

webAppBuilder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true) 
    .AddEntityFrameworkStores<GearBoxDbContext>();
webAppBuilder.Services.AddRazorPages();
webAppBuilder.Services.AddSignalR();
webAppBuilder.Services
    .AddSingleton(gameBuilder.Items)
    .AddSingleton<IPlayerCharacterRepository, PlayerCharacterRepository>()
    .AddSingleton<GameServer>()
    .AddSingleton(game)
    .Configure<EmailConfig>(webAppBuilder.Configuration.GetSection(EmailConfig.ConfigSection))
    .AddTransient<IEmailSender, EmailSender>();

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

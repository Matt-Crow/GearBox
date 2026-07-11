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
using GearBox.Core.Model.Abilities.Passives.Impl;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;

/*
    Actives and passives cannot be stored in a JSON file,
    as they contain executable code,
    so they are initialized outside of the GameResourceLoader.

    Items can provide actives and passives,
    and thus must be loaded after loading actives and passives.

    Each area depends on game-wide resources,
    such as items,
    so each area is loaded after all game-wide resources have been loaded.
*/

// need to grab configuration before most other things
var webAppBuilder = WebApplication.CreateBuilder(args);

var gearboxConfig = new GearBoxConfig();
webAppBuilder.Configuration
    .GetSection("GearBox")
    .Bind(gearboxConfig);
var rng = new RandomNumberGenerator();
var actives = new List<IActiveAbility>()
{
    new Cleave(),
    new LaserBolt()
};
var passives = new List<IPassiveAbility>()
{
    Armored.Lightly(),
    Armored.Moderately(),
    Armored.Heavily(),
    Ranged.Moderately(),
    Ranged.Long(),
    new Intangible(),
    new Levitate(),
    new Spikey()
};
var gameBuilder = new GameBuilder(gearboxConfig, rng, actives, passives);


var resourceLoader = new GameResourceLoader(gameBuilder.Actives, gameBuilder.Passives, rng);
await resourceLoader.LoadResourcesInto(gameBuilder);



// we have all the game data, now make areas in that game
var bazaarMap = await resourceLoader.LoadMapByName("bazaar");
var desertMap = await resourceLoader.LoadMapByName("desert");
var canyonMap = await resourceLoader.LoadMapByName("canyon");
gameBuilder
    .WithArea("desert", 1, area => area
        .AddLoot(loot => loot
            .AddOption(new LootOption(gameBuilder.Items.MakeOrThrow("Stone")))
            .AddOption(new LootOption(gameBuilder.Items.MakeOrThrow("Bronze")))
            .AddOption(new LootOption(gameBuilder.Items.MakeOrThrow("Spiney Helm")))
            .AddOption(new LootOption(Grade.COMMON, new Gold(5)))
            .AddOption(new LootOption(Grade.UNCOMMON, new Gold(10)))
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
        .AddShop("Starter Part Shop", Coordinates.FromTiles(2, 7), Color.BLUE, shop => shop
            .AddItem("Hard Hat")
            .AddItem("Laser Lenses")
            .AddItem("Armored Treads")
            .AddItem("Rotowheel")
            .AddItem("Training Club")
            .AddItem("Training Blaster")
            .AddItem("Tanky Torso")
            .AddItem("High-Capacity Torso")
        )
        .WithExit(BorderExit.Top("desert"))
    )
    .WithArea("canyon", 2, area => area
        .AddLoot(loot => loot
            .AddOption(new LootOption(gameBuilder.Items.MakeOrThrow("Bronze")))
            .AddOption(new LootOption(gameBuilder.Items.MakeOrThrow("Silver")))
            .AddOption(new LootOption(gameBuilder.Items.MakeOrThrow("Antigravity Thrusters")))
            .AddOption(new LootOption(Grade.RARE, new Gold(25)))
        )
        .AddEnemies(enemies => enemies
            .Add("Snake")
            .Add("Jackal")
            .Add("Specter")
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

using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;

var bazaarMap = await GameResourceLoader.LoadMapByName("bazaar");
var desertMap = await GameResourceLoader.LoadMapByName("desert");
var canyonMap = await GameResourceLoader.LoadMapByName("canyon");
var itemResources = await GameResourceLoader.LoadAllItems();

var webAppBuilder = WebApplication.CreateBuilder(args);
var gearboxConfig = new GearBoxConfig();
webAppBuilder.Configuration
    .GetSection("GearBox")
    .Bind(gearboxConfig);

var game = new GameBuilder(gearboxConfig)
    .DefineItems(items =>
    {
        foreach (var item in itemResources)
        {
            items = items.Add(item);
        }
        return items;
    })
    .AddCraftingRecipe(recipe => recipe
        .And("Bronze", 25)
        .Makes("Bronze Khopesh")
    )
    .AddCraftingRecipe(recipe => recipe
        .And("Bronze", 25)
        .Makes("Bronze Armor")
    )
    .DefineEnemies(enemies => enemies
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
        )
    )
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
    )
    .Build();

// Add services to the container.
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

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<GameHub>("/area-hub");

app.Run();

using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;

var bazaarMap = await GameResourceLoader.LoadMapByName("bazaar");
var desertMap = await GameResourceLoader.LoadMapByName("desert");
var canyonMap = await GameResourceLoader.LoadMapByName("canyon");

var webAppBuilder = WebApplication.CreateBuilder(args);

var game = new GameBuilder()
    .DefineItems(items => items
        .Add(ItemUnion.Of(new Material("Stone", Grade.COMMON, "A low-grade mining material, but it's better than nothing.")))
        .Add(ItemUnion.Of(new Material("Bronze", Grade.UNCOMMON, "Used to craft low-level melee equipment")))
        .Add(ItemUnion.Of(new Material("Silver", Grade.RARE, "Used to craft enhancements for your equipment.")))
        .Add(ItemUnion.Of(new Material("Gold", Grade.EPIC, "Used to craft powerful magical artifacts.")))
        .Add(ItemUnion.Of(new Material("Titanium", Grade.LEGENDARY, "A high-grade mining material for crafting powerful melee equipment.")))
        .Add(ItemUnion.Of(new Equipment<WeaponStats>("Training Sword", new WeaponStats(AttackRange.MELEE), Grade.COMMON, new()
            {
                {PlayerStatType.OFFENSE, 1},
                {PlayerStatType.MAX_HIT_POINTS, 1}
            }))
        )
        .Add(ItemUnion.Of(new Equipment<WeaponStats>("Training Bow", new WeaponStats(AttackRange.LONG), Grade.COMMON, new()
            {
                {PlayerStatType.OFFENSE, 1},
                {PlayerStatType.SPEED, 1}
            })) 
        )
        .Add(ItemUnion.Of(new Equipment<WeaponStats>("Training Staff", new WeaponStats(AttackRange.MEDIUM), Grade.COMMON, new()
            {
                {PlayerStatType.MAX_HIT_POINTS, 1},
                {PlayerStatType.MAX_ENERGY, 1}
            }))
        )
        .Add(ItemUnion.Of(new Equipment<ArmorStats>("Fighter Initiate's Armor", new ArmorStats(ArmorClass.HEAVY), Grade.COMMON, new()
            {
                {PlayerStatType.MAX_HIT_POINTS, 1},
                {PlayerStatType.OFFENSE, 1}
            }))
        )
        .Add(ItemUnion.Of(new Equipment<ArmorStats>("Archer Initiate's Armor", new ArmorStats(ArmorClass.MEDIUM), Grade.COMMON, new()
            {
                {PlayerStatType.SPEED, 1},
                {PlayerStatType.OFFENSE, 1}
            }))
        )
        .Add(ItemUnion.Of(new Equipment<ArmorStats>("Mage Initiate's Armor", new ArmorStats(ArmorClass.LIGHT), Grade.COMMON, new()
            {
                {PlayerStatType.MAX_ENERGY, 1},
                {PlayerStatType.OFFENSE, 1}
            }))
        )
        .Add(ItemUnion.Of(new Equipment<WeaponStats>("Bronze Khopesh", new WeaponStats(AttackRange.MELEE), Grade.UNCOMMON, new()
            {
                {PlayerStatType.OFFENSE, 2},
                {PlayerStatType.MAX_HIT_POINTS, 1}
            }))
        )
        .Add(ItemUnion.Of(new Equipment<ArmorStats>("Bronze Armor", new ArmorStats(ArmorClass.HEAVY), Grade.UNCOMMON, new()
            {
                {PlayerStatType.OFFENSE, 1},
                {PlayerStatType.MAX_HIT_POINTS, 2},
                {PlayerStatType.MAX_ENERGY, 1}
            }))
        )
        .Add(ItemUnion.Of(new Equipment<WeaponStats>("Fang", new WeaponStats(AttackRange.MELEE), Grade.COMMON, new()
            {
                {PlayerStatType.OFFENSE, 2},
                {PlayerStatType.SPEED, 1}
            }))
        )
    )
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

using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;

var desertMap = await GameResourceLoader.LoadMapByName("desert");
var canyonMap = await GameResourceLoader.LoadMapByName("canyon");

var webAppBuilder = WebApplication.CreateBuilder(args);

var game = new GameBuilder()
    .DefineItems(items => items
        .Add(ItemUnion.Of(new Material(new ItemType("Stone", Grade.COMMON), "A low-grade mining material, but it's better than nothing.")))
        .Add(ItemUnion.Of(new Material(new ItemType("Bronze", Grade.UNCOMMON), "Used to craft low-level melee equipment")))
        .Add(ItemUnion.Of(new Material(new ItemType("Silver", Grade.RARE), "Used to craft enhancements for your equipment.")))
        .Add(ItemUnion.Of(new Material(new ItemType("Gold", Grade.EPIC), "Used to craft powerful magical artifacts.")))
        .Add(ItemUnion.Of(new Material(new ItemType("Titanium", Grade.LEGENDARY), "A high-grade mining material for crafting powerful melee equipment.")))
        .Add(ItemUnion.Of(new WeaponBuilder(new ItemType("Training Sword", Grade.COMMON))
            .WithRange(AttackRange.MELEE)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.OFFENSE, 1},
                {PlayerStatType.MAX_HIT_POINTS, 1}
            }).Build(1)) // todo use area level
        )
        .Add(ItemUnion.Of(new WeaponBuilder(new ItemType("Training Bow", Grade.COMMON))
            .WithRange(AttackRange.LONG)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.OFFENSE, 1},
                {PlayerStatType.SPEED, 1}
            }).Build(1)) // todo use area level
        )
        .Add(ItemUnion.Of(new WeaponBuilder(new ItemType("Training Staff", Grade.COMMON))
            .WithRange(AttackRange.MEDIUM)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.MAX_HIT_POINTS, 1},
                {PlayerStatType.MAX_ENERGY, 1}
            }).Build(1)) // todo use area level
        )
        .Add(ItemUnion.Of(new ArmorBuilder(new ItemType("Fighter Initiate's Armor", Grade.COMMON))
            .WithArmorClass(ArmorClass.HEAVY)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.MAX_HIT_POINTS, 1},
                {PlayerStatType.OFFENSE, 1}
            }).Build(1)) // todo use area level
        )
        .Add(ItemUnion.Of(new ArmorBuilder(new ItemType("Archer Initiate's Armor", Grade.COMMON))
            .WithArmorClass(ArmorClass.MEDIUM)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.SPEED, 1},
                {PlayerStatType.OFFENSE, 1}
            }).Build(1)) // todo use area level
        )
        .Add(ItemUnion.Of(new ArmorBuilder(new ItemType("Mage Initiate's Armor", Grade.COMMON))
            .WithArmorClass(ArmorClass.LIGHT)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.MAX_ENERGY, 1},
                {PlayerStatType.OFFENSE, 1}
            }).Build(1)) // todo use area level
        )
        .Add(ItemUnion.Of(new WeaponBuilder(new ItemType("Bronze Khopesh", Grade.UNCOMMON))
            .WithRange(AttackRange.MELEE)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.OFFENSE, 2},
                {PlayerStatType.MAX_HIT_POINTS, 1}
            }).Build(1)) // craft at level 1 so players don't just grind lv 20 weapons in lv 1 area
        )
        .Add(ItemUnion.Of(new ArmorBuilder(new ItemType("Bronze Armor", Grade.UNCOMMON))
            .WithArmorClass(ArmorClass.HEAVY)
            .WithStatWeights(new Dictionary<PlayerStatType, int>()
            {
                {PlayerStatType.OFFENSE, 1},
                {PlayerStatType.MAX_HIT_POINTS, 2},
                {PlayerStatType.MAX_ENERGY, 1}
            }).Build(1)) // craft at level 1 so players don't just grind lv 20 weapons in lv 1 area
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
    .WithArea("desert", area => area
        .AddLoot(loot => loot
            .AddItem("Training Sword")
            .AddItem("Training Bow")
            .AddItem("Training Staff")
            .AddItem("Fighter Initiate's Armor")
            .AddItem("Archer Initiate's Armor")
            .AddItem("Mage Initiate's Armor")
            .AddItem("Stone")
            .AddItem("Bronze")
            .Add(Grade.COMMON, new Gold(5))
            .Add(Grade.UNCOMMON, new Gold(10))
        )
        .AddDefaultEnemies()
        .WithMap(desertMap)
        .WithExit(BorderExit.Right("canyon"))
    )
    .WithArea("canyon", area => area
        .AddLoot(loot => loot
            .AddItem("Bronze")
            .AddItem("Silver")
            .Add(Grade.RARE, new Gold(25))
        )
        .AddDefaultEnemies()
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

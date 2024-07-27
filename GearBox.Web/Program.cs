using GearBox.Core.Model;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Static;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;

var desertMap = await GameResourceLoader.LoadMapByName("desert");
var canyonMap = await GameResourceLoader.LoadMapByName("canyon");

var webAppBuilder = WebApplication.CreateBuilder(args);

var game = new GameBuilder()
    .WithArea("desert", area => area
        .AddMiningSkill()
        .AddStarterEquipment()
        .AddLoot(loot => loot
            .AddGold(Grade.COMMON, new Gold(5))
            .AddGold(Grade.UNCOMMON, new Gold(10))
        )
        .AddDefaultEnemies()
        .WithMap(desertMap)
        .WithExit(BorderExit.Right("canyon"))
    )
    .WithArea("canyon", area => area
        .AddMiningSkill()
        .AddLoot(loot => loot
            .AddGold(Grade.RARE, new Gold(25))
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

using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;

var webAppBuilder = WebApplication.CreateBuilder(args);

var game = new GameBuilder()
    .WithArea(area => area
        .AddMiningSkill()
        .AddStarterEquipment()
        .AddDefaultEnemies()
        .WithDesertMap()
    )
    .Build();
var area = game.GetDefaultArea();

// testing LootChests
area.AddTimer(new GameTimer(area.SpawnLootChest, 50));

// testing EnemySpawner
var spawner = new EnemySpawner(area, new EnemySpawnerOptions()
{
    WaveSize = 3,
    MaxChildren = 10
});
area.AddTimer(new GameTimer(spawner.SpawnWave, Duration.FromSeconds(10).InFrames));

// Add services to the container.
webAppBuilder.Services.AddRazorPages();
webAppBuilder.Services.AddSignalR();
webAppBuilder.Services.AddSingleton(new GameServer(game)); // todo Repository

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

using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var world = new WorldBuilder()
    .AddMiningSkill()
    .AddStarterEquipment()
    .AddDefaultEnemies()
    .WithDesertMap()
    .Build();

// testing LootChests
world.AddTimer(new GameTimer(world.SpawnLootChest, 50));

// testing EnemySpawner
var spawner = new EnemySpawner(world, new EnemySpawnerOptions()
{
    WaveSize = 3,
    MaxChildren = 10
});
world.AddTimer(new GameTimer(spawner.SpawnWave, Duration.FromSeconds(10).InFrames));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSingleton(new AreaServer(world)); // todo AreaRepository

var app = builder.Build();

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
app.MapHub<AreaHub>("/area-hub");

app.Run();

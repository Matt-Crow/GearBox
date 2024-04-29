using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var world = new WorldBuilder()
    .AddMiningSkill()
    .AddStarterWeapons()
    .AddDefaultEnemies()
    .WithDesertMap()
    .Build();

// testing LootChests
world.AddTimer(new WorldTimer(() => world.SpawnLootChest(), 50));

// testing EnemySpawner
world.DynamicContent.AddDynamicObject(new EnemySpawner(
    world, 
    new EnemySpawnerOptions()
    {
        WaveSize = 3,
        MaxChildren = 10
    }
));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSingleton(new WorldServer(world)); // todo WorldRepository

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
app.MapHub<WorldHub>("/world-hub");

app.Run();

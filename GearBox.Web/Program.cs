using GearBox.Core.Model;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;

var map = new Map();
map.SetTileTypeForKey(1, TileType.Tangible(Color.RED));
map.SetTileAt(Coordinates.FromTiles(5, 5), 1);
map.SetTileAt(Coordinates.FromTiles(5, 6), 1);
map.SetTileAt(Coordinates.FromTiles(6, 5), 1);
map.SetTileAt(Coordinates.FromTiles(8, 5), 1);
var world = new World(Guid.NewGuid(), new StaticWorldContent(map, new List<IStaticGameObject>()));

var builder = WebApplication.CreateBuilder(args);

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<WorldHub>("/world-hub");

app.Run();

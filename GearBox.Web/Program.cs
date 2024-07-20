using GearBox.Core.Model;
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
        .AddDefaultEnemies()
        .WithMap(desertMap)
        .WithExit(BorderExit.Right("canyon"))
    )
    .WithArea("canyon", area => area
        .AddMiningSkill()
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

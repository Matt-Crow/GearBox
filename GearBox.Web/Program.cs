using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.Abilities.Actives.Impl;
using GearBox.Core.Model.Items;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;
using GearBox.Web.Database;
using GearBox.Web.Email;
using GearBox.Core.Model.GameObjects.Player;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using GearBox.Core.Utils;
using GearBox.Core.Model.Abilities.Passives.Impl;
using GearBox.Core.Model.ResourcePacks;

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

var gameBuilder = new GameBuilder(
    gearboxConfig, 
    rng, 
    new GameResources()
    {
        Actives = [
            new Cleave(),
            new LaserBolt()
        ],
        Passives = [
            Armored.Lightly(),
            Armored.Moderately(),
            Armored.Heavily(),
            Ranged.Moderately(),
            Ranged.Long(),
            new Intangible(),
            new Levitate(),
            new Spikey()
        ],
        ResourcePacks = [
            await GameResourceLoader.LoadDefaultResourcePack()
        ]
    }
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

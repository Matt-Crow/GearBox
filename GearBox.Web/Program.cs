using GearBox.Core.Model;
using GearBox.Core.Server;
using GearBox.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var world = new WorldBuilder()
    .AddMiningSkill()
    .AddStarterWeapon()
    .WithDummyMap()
    .Build();

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

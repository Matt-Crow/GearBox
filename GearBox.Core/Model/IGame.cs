using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json.GameInit;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model;

/// <summary>
/// top level model in which everything happens
/// </summary>
public interface IGame
{
    Factory<ItemUnion> Items { get; }
    Crafter Crafter { get; }
    

    void AddArea(IArea area);

    IArea GetDefaultArea();

    IArea? GetAreaByName(string name);

    GameInitJson GetGameInitJsonFor(PlayerCharacter player);

    void Update();
}
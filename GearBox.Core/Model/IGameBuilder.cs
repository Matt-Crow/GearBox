using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model;

public interface IGameBuilder
{
    /// <summary>
    /// A reference to the actives available in the game this is building.
    /// </summary>
    Factory<IActiveAbility> Actives { get; }

    /// <summary>
    /// A reference to the passives available in the game this is building.
    /// </summary>
    Factory<IPassiveAbility> Passives { get; }

    /// <summary>
    /// A reference to the items available in the game this is building.
    /// </summary>
    IItemFactory Items { get;}

    /// <summary>
    /// A reference to the enemies which can be encountered in the game this is building.
    /// </summary>
    IEnemyRepository Enemies { get; }

    /// <summary>
    /// Defines an area in the game. The name must be unique within the game.
    /// </summary>
    IGameBuilder WithArea(string name, int level, Func<AreaBuilder, AreaBuilder> defineArea);
    
    IGame Build();
}
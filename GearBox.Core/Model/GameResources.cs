using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.ResourcePacks;

namespace GearBox.Core.Model;

/// <summary>
/// Raw resources available to a game.
/// This is as opposed to non-raw resources such as enemies moving around
/// and non-resources such as random number generators.
/// </summary>
public class GameResources
{
    public List<IActiveAbility> Actives { get; set; } = [];
    public List<IPassiveAbility> Passives { get; set; } = [];
    public List<ResourcePack> ResourcePacks { get; set; } = [];
}
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;

namespace GearBox.Web.Model.Json;

public static class ItemJsonUtils
{
    public static Grade GetGradeByName(string name)
    {
        var result = Grade.GetGradeByName(name) ?? throw new Exception($"Invalid grade name: \"{name}\"");
        return result;
    }

    public static Dictionary<PlayerStatType, int> GetPlayerStats(Dictionary<string, int> stats)
    {
        var result = new Dictionary<PlayerStatType, int>();
        foreach (var kv in stats)
        {
            var statType = PlayerStatType.GetPlayerStatTypeByName(kv.Key) ?? throw new ArgumentException($"Invalid Stats key: \"{kv.Key}\"");
            result[statType] = kv.Value;
        }
        return result;
    }

    public static IEnumerable<IActiveAbility> GetActives(IActiveAbilityFactory factory, List<string> activeNames)
    {
        var result = activeNames
            .Select(name => factory.Make(name) ?? throw new ArgumentException($"Invalid active name: \"{name}\""))
            .ToList();
        return result;
    }
}
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerStatSummary 
{
    private readonly IEnumerable<PlayerStatSummaryElement> _elements = [
        new(
            p => $"Level: {p.Level}",
            p => [p.Level]
        ),
        new(
            p => $"XP: {p.Xp}/{p.XpToNextLevel}",
            p => [p.Xp, p.XpToNextLevel]
        ),
        new(
            p => $"HP: {p.HitPointsRemaining}/{p.MaxHitPoints}",
            p => [p.HitPointsRemaining, p.MaxHitPoints]
        ),
        new(
            p => $"Energy: {p.EnergyRemaining}/{p.MaxEnergy}",
            p => [p.EnergyRemaining, p.MaxEnergy]
        ),
        new(
            p => $"Offense: {p.Stats.Offense.Points} (+{Percent(p.Stats.Offense.Value)} damage dealt)",
            p => [p.Stats.Offense.Points]
        ),
        new(
            p => $"Speed: {p.Stats.Speed.Points} (+{Percent(p.Stats.Speed.Value)} movement speed)",
            p => [p.Stats.Speed.Points]
        ),
        new(
            p => $"Armor class: {p.ArmorClass} (-{Percent(p.ArmorClass.DamageReduction)} damage taken)",
            p => [p.ArmorClass]
        )
    ];

    private readonly PlayerCharacter _player;

    public PlayerStatSummary(PlayerCharacter player)
    {
        _player = player;
    }

    private static string Percent(double number) => $"{(int)(number * 100)}%";

    public PlayerStatSummaryJson ToJson()
    {
        var lines = _elements
            .Select(e => e.ToString(_player))
            .ToList();
        var json = new PlayerStatSummaryJson(lines);
        return json;
    }
}
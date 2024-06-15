using System.Text.Json;
using GearBox.Core.Model.GameObjects.ChangeTracking;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerStatSummary : IDynamic
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
    private readonly ChangeTracker _changeTracker;

    public PlayerStatSummary(PlayerCharacter player)
    {
        _player = player;
        _changeTracker = new(this);
    }

    public IEnumerable<object?> DynamicValues => _elements.SelectMany(e => e.DynamicValues(_player));

    public string Serialize(SerializationOptions options)
    {
        var lines = _elements
            .Select(e => e.ToString(_player))
            .ToList();
        var json = new PlayerStatSummaryJson(lines);
        return JsonSerializer.Serialize(json, options.JsonSerializerOptions);
    }

    private static string Percent(double number)
    {
        return $"{(int)(number*100)}%";
    }

    public void Update() => _changeTracker.Update();
    public StableJson ToJson() => _changeTracker.ToJson();
}
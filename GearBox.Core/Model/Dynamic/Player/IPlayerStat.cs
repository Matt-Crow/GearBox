namespace GearBox.Core.Model.Dynamic.Player;

// allows PlayerStatType to get around issues with generics: https://stackoverflow.com/a/15575106
public interface IPlayerStat
{
    int Points { get; set; }
}
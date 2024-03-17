using GearBox.Core.Model;
using GearBox.Core.Model.Stable;

namespace GearBox.Core.Controls;

public interface IControlCommand
{
    void ExecuteOn(PlayerCharacter target, World inWorld);
}
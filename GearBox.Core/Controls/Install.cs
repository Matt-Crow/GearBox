using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

public class Install : IControlCommand
{
    private readonly Guid _partId;

    public Install(Guid partId)
    {
        _partId = partId;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        target.InstallById(_partId);
    }
}
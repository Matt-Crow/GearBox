using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Controls;

public interface IControlCommand
{
    public void ExecuteOn(Character target);
}
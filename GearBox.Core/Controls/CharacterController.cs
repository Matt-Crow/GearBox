using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Controls;

public class CharacterController
{
    // this isn't like Orpheus where Characters are constantly serialized & deserialized
    private readonly Character _target;

    public CharacterController(Character target)
    {
        _target = target;
    }

    /// <summary>
    /// Takes a command from a user and executes it on the target.
    /// </summary>
    /// <param name="command">a command from a user</param>
    public void Receive(IControlCommand command)
    {
        command.ExecuteOn(_target);
    }
}
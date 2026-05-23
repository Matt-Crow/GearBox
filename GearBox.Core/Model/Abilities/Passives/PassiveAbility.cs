using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Model.Abilities.Passives;

public abstract class PassiveAbility : IPassiveAbility
{
    public PassiveAbility(string name)
    {
        Name = name;
    }

    public string Name { get; init; }

    public Character? User { get; private set; }

    public void SetUser(Character? newUser)
    {
        if (User != null)
        {
            UnregisterFrom(User);
        }
        if (newUser != null)
        {
            User = newUser;
            RegisterTo(newUser);
        }
    }

    /// <summary>
    /// Removes event listeners from the old user
    /// </summary>
    protected abstract void UnregisterFrom(Character user);

    /// <summary>
    /// Adds event listeners to the new user
    /// </summary>
    protected abstract void RegisterTo(Character user);

    public abstract string GetDescription();

    public virtual void Update()
    {
        
    }

    public abstract IPassiveAbility Copy();
}
namespace GearBox.Core.Model.Stable.Items;

public class WeaponBuilder
{
    private readonly ItemType _type;
    private string? _description;
    private readonly WeaponStatWeights _statWeights = new();

    public WeaponBuilder(ItemType type)
    {
        _type = type;
    }

    public WeaponBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public WeaponBuilder WithStatWeights(Action<WeaponStatWeights> action)
    {
        action(_statWeights);
        return this;
    }

    public Weapon Build()
    {
        var result = new Weapon(
            _type, 
            _description,
            null, // id is null
            _statWeights.Build()
        );
        return result;
    }
}
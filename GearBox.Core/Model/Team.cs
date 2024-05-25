namespace GearBox.Core.Model;

public class Team
{
    private readonly string _name;

    public Team(string name="no team")
    {
        _name = name;
    }

    public override bool Equals(object? obj)
    {
        return obj is Team other && other._name == _name;
    }

    public override int GetHashCode()
    {
        return _name.GetHashCode();
    }

    public override string ToString()
    {
        return $"Team {_name}";
    }
}
using System.Text.Json;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// Projectiles are generated by characters when they use attacks, and can inflict damage
/// </summary>
public class Projectile : IGameObject
{
    private readonly MobileBehavior _mobility;
    private readonly Distance _range;
    private readonly Attack _attack;
    private double _distanceTraveledInPixels = 0;
    private bool _terminating; 

    public Projectile(Coordinates coordinates, Velocity velocity, Distance range, Attack attack, Color color)
    {
        Body = new(Distance.FromTiles(0.25))
        {
            Location = coordinates
        };
        _attack = attack;
        Body.Collided += HandleCollision;
        _mobility = new(Body, velocity);
        _mobility.StartMovingIn(velocity.Angle); // MobileBehavior defaults to not moving
        _range = range;
        Serializer = new("projectile", Serialize);
        Termination = new(this, () => _terminating || _range.InPixels <= _distanceTraveledInPixels);
        Color = color;
    }


    public Serializer Serializer { get; init; }
    public BodyBehavior Body { get; init; }
    public TerminateBehavior Termination { get; init; }
    private Color Color { get; init; }
    

    private void HandleCollision(object? sender, CollideEventArgs e)
    {
        if (!_terminating && _attack.CanResolveAgainst(e.CollidedWith))
        {
            _attack.HandleCollision(sender, e);
            _terminating = true;
        }
    }

    public void Terminate()
    {
        _terminating = true;
    }
    
    public void Update()
    {
        _mobility.UpdateMovement();
        _distanceTraveledInPixels += _mobility.Velocity.Magnitude.InPixelsPerFrame;
    }

    private string Serialize(SerializationOptions options)
    {
        var json = new ProjectileJson(
            Body.Location.XInPixels, 
            Body.Location.YInPixels,
            Body.Radius.InPixels,
            _mobility.Velocity.Angle.BearingInDegrees,
            Color.ToJson()
        );
        return JsonSerializer.Serialize(json, options.JsonSerializerOptions);
    }
}
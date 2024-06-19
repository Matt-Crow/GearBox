using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Tests.Model.GameObjects;

public class GameObjectSpy : IGameObject
{
    private int _timesUpdated = 0;
    public int TimesUpdated => _timesUpdated;
    public bool HasBeenUpdated => _timesUpdated > 0;
    public Serializer? Serializer => null;
    public BodyBehavior? Body { get; init; } = null;
    public TerminateBehavior? Termination => null;

    public void Update()
    {
        _timesUpdated++;
    }
}
using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Tests.Model.Dynamic;

public class DynamicGameObjectSpy : IDynamicGameObject
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
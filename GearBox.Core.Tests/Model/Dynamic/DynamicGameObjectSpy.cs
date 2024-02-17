using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Tests.Model.Dynamic;

public class DynamicGameObjectSpy : IDynamicGameObject
{
    private int _timesUpdated = 0;

    public int TimesUpdated { get => _timesUpdated; }
    public bool HasBeenUpdated { get => _timesUpdated > 0; }

    public BodyBehavior? Body { get; init; } = null;

    public IDynamicGameObjectJson ToJson()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        _timesUpdated++;
    }
}
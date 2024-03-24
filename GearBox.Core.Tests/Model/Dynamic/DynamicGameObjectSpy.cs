using System.Text.Json;
using GearBox.Core.Model.Dynamic;

namespace GearBox.Core.Tests.Model.Dynamic;

public class DynamicGameObjectSpy : IDynamicGameObject
{
    private int _timesUpdated = 0;

    public string Type => "";
    public int TimesUpdated => _timesUpdated;
    public bool HasBeenUpdated => _timesUpdated > 0;

    public BodyBehavior? Body { get; init; } = null;
    public bool IsTerminated => false;

    public void Update()
    {
        _timesUpdated++;
    }

    public string Serialize(JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
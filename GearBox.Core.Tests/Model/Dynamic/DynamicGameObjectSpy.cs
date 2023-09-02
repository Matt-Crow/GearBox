namespace GearBox.Core.Tests.Model.Dynamic;

using GearBox.Core.Model.Dynamic;

public class DynamicGameObjectSpy : IDynamicGameObject
{
    private int _timesUpdated = 0;

    public int TimesUpdated { get => _timesUpdated; }
    public bool HasBeenUpdated { get => _timesUpdated > 0; }

    public IDynamicGameObjectJson ToJson()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        _timesUpdated++;
    }
}
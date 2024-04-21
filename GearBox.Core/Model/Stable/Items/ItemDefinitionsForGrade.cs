namespace GearBox.Core.Model.Stable.Items;

// could refactor this into Inventory if ItemDefinition is not needed
public class ItemDefinitionsForGrade
{
    private readonly List<Equipment> _equipment = [];
    private readonly List<Material> _materials = [];

    public void AddEquipment(Equipment item)
    {
        _equipment.Add(item);
    }

    public void AddMaterial(Material material)
    {
        _materials.Add(material);
    }

    public void AddRandomItemTo(Inventory inventory)
    {
        if (!HasAnyDefinitions())
        {
            throw new InvalidOperationException("ItemDefinitionForGrade has no items");
        }

        var addMaterial = _equipment.Count == 0 || (Random.Shared.Next() & 1) == 0;
        if (addMaterial && _materials.Count > 0)
        {
            var x = Random.Shared.Next(_materials.Count);
            inventory.Materials.Add(_materials[x].ToOwned());
        }
        else
        {
            var x = Random.Shared.Next(_equipment.Count);
            inventory.Equipment.Add(_equipment[x].ToOwned());
        }
    }

    public bool HasAnyDefinitions()
    {
        return _equipment.Count + _materials.Count > 0;
    }
}
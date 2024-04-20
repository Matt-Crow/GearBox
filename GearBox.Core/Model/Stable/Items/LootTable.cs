namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// Used to determine what items players can get from LootChests
/// </summary>
public class LootTable
{
    private readonly Dictionary<Grade, ItemDefinitionsForGrade> _values = Grade.ALL.ToDictionary(x => x, _ => new ItemDefinitionsForGrade());

    public void AddEquipment(ItemDefinition<Equipment> itemDefinition)
    {
        _values[itemDefinition.Type.Grade].AddEquipment(itemDefinition);
    }

    public void AddMaterial(ItemDefinition<Material> itemDefinition)
    {
        _values[itemDefinition.Type.Grade].AddMaterial(itemDefinition);
    }

    public Inventory GetRandomItems()
    {
        var result = new Inventory();
        var numItems = Random.Shared.Next(0, 3) + 1;
        for (int i = 0; i < numItems; i++)
        {
            AddRandomItemTo(result);
        }
        return result;
    }

    private void AddRandomItemTo(Inventory inventory)
    {
        var grade = ChooseRandomGrade();
        var definitions = _values[grade];
        definitions.AddRandomItemTo(inventory);
    }

    private Grade ChooseRandomGrade()
    {
        var options = _values
            .Where(x => x.Value.HasAnyDefinitions())
            .Select(x => x.Key)
            .OrderBy(k => k.Order)
            .ToList();
        
        if (options.Count == 0)
        {
            throw new InvalidOperationException($"LootTable has no items");
        }

        /*
            Suppose we have 3 grades with weights 10, 20, and 40.
            Probabilities of being chosen should be 10/70, 20/70, and 40/70
            So choose a number between 0-69.
             0-10 => 10
            10-30 => 20
            30-70 => 40
            suppose i = 25, then it should select the grade with a weight of 20
        */
        var totalWeight = options.Sum(grade => grade.Weight);
        var randomNumber = Random.Shared.Next(totalWeight);
        foreach (var grade in options)
        {
            if (grade.Weight > randomNumber)
            {
                return grade;
            }
            randomNumber -= grade.Weight;
        }
        throw new Exception("Something went wrong when chosing a random grade");
    }
}
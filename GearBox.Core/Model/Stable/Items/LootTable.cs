namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// Used to determine what items players can get from LootChests
/// </summary>
public class LootTable
{
    private readonly Dictionary<Grade, Inventory> _values = Grade.ALL.ToDictionary(x => x, _ => new Inventory());

    public void AddEquipment(Equipment itemDefinition)
    {
        _values[itemDefinition.Type.Grade].Equipment.Add(itemDefinition);
    }

    public void AddMaterial(Material itemDefinition)
    {
        _values[itemDefinition.Type.Grade].Materials.Add(itemDefinition);
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
        AddRandomItemFromGrade(grade, inventory);
    }

    private Grade ChooseRandomGrade()
    {
        var options = _values
            .Where(x => x.Value.Any())
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

    private void AddRandomItemFromGrade(Grade grade, Inventory destination)
    {
        var source = _values[grade];
        var equipment = GetRandomItemFrom(source.Equipment) as Equipment; // change once generic
        var material = GetRandomItemFrom(source.Materials) as Material; // change once generic
        var options = new List<AddItemCommand>()
        {
            new AddItemCommand(equipment?.ToOwned(), (item, inventory) => inventory.Equipment.Add(item)),
            new AddItemCommand(material?.ToOwned(), (item, inventory) => inventory.Materials.Add(item))
        };
        var possibleOptions = options
            .Where(option => option.IsPossible())
            .ToList();
        
        if (!possibleOptions.Any())
        {
            throw new InvalidOperationException("source inventory has no items");
        }

        var i = Random.Shared.Next(possibleOptions.Count);
        possibleOptions[i].ExecuteOn(destination);
    }

    private static IItem? GetRandomItemFrom(InventoryTab tab)
    {
        var options = tab.Content.AsEnumerable()
            .Where(stack => stack.Quantity > 0)
            .Select(stack => stack.Item)
            .ToList();
        if (!options.Any())
        {
            return null;
        }
        var i = Random.Shared.Next(options.Count);
        return options[i];
    }

    private class AddItemCommand
    {
        private readonly IItem? _value;
        private readonly Action<IItem, Inventory> _action;

        public AddItemCommand(IItem? value, Action<IItem, Inventory> action)
        {
            _value = value;
            _action = action;
        }

        public bool IsPossible()
        {
            return _value != null;
        } 

        public void ExecuteOn(Inventory destination)
        {
            if (_value != null)
            {
                _action.Invoke(_value, destination);
            }
        }
    }
}
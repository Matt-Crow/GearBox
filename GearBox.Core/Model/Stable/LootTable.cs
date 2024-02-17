namespace GearBox.Core.Model.Stable;

/// <summary>
/// Used to determine what items players can get from LootChests
/// </summary>
public class LootTable
{
    private readonly Dictionary<Grade, List<ItemDefinition>> _gradeToDefinitions = Grade.ALL.ToDictionary(x => x, _ => new List<ItemDefinition>());

    public void Add(ItemDefinition itemDefinition)
    {
        _gradeToDefinitions[itemDefinition.Type.Grade].Add(itemDefinition);
    }

    public IItem GetRandomItem()
    {
        var grade = ChooseRandomGrade();
        var bucket = _gradeToDefinitions[grade];
        var i = Random.Shared.Next(bucket.Count);
        var result = bucket[i].Create();
        return result;
    }

    private Grade ChooseRandomGrade()
    {
        var options = _gradeToDefinitions
            .Where(kv => kv.Value.Any())
            .Select(kv => kv.Key)
            .OrderBy(k => k.Order)
            .ToList();
        
        if (!options.Any())
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
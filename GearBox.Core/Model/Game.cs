using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json.GameInit;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model;

public class Game : IGame
{
    private readonly List<IArea> _areas = [];
    private readonly Factory<CraftingRecipe> _craftingRecipes;

    public Game(Factory<CraftingRecipe>? craftingRecipes = null)
    {
        _craftingRecipes = craftingRecipes ?? Factory<CraftingRecipe>.Of(cr => cr, []);
        Crafter = new Crafter(_craftingRecipes);
    }


    public Crafter Crafter { get; init; }


    // cannot create game & area at the same time due to circular dependency
    public void AddArea(IArea area)
    {
        _areas.Add(area);
    }

    public IArea GetDefaultArea()
    {
        // todo some other way of signifying default area
        return _areas.FirstOrDefault() ?? throw new Exception("Game has no area");
    }

    public IArea? GetAreaByName(string name) => _areas.Find(a => a.Name == name);


    public GameInitJson GetGameInitJsonFor(PlayerCharacter player)
    {
        var result = new GameInitJson(
            player.Id,
            _craftingRecipes.AllValues
                .Select(recipe => recipe.ToJson())
                .ToList()
        );
        return result;
    }

    public void Update()
    {
        foreach (var area in _areas)
        {
            area.Update();
        }
    }
}
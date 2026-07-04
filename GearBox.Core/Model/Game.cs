using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Model.Json.GameInit;

namespace GearBox.Core.Model;

public class Game : IGame
{
    private readonly List<IArea> _areas = [];
    private readonly IItemFactory _items;
    private readonly CraftingRecipeRepository _craftingRecipes;

    public Game(IItemFactory? items = null, CraftingRecipeRepository? craftingRecipes = null)
    {
        _items = items ?? new ItemFactory();
        _craftingRecipes = craftingRecipes ?? CraftingRecipeRepository.Empty();
        Crafter = new Crafter(_items, _craftingRecipes);
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
            _craftingRecipes.All
                .Select(ToJson)
                .ToList()
        );
        return result;
    }

    private CraftingRecipeJson ToJson(CraftingRecipeDTO craftingRecipe)
    {
        var ingredients = craftingRecipe.Ingredients
            .Select(dto => new ItemStack<Material>(_items.MakeMaterial(dto.ItemName), dto.Quantity))
            .Select(stack => stack.ToJson())
            .ToList();
        var makes = _items.MakeOrThrow(craftingRecipe.ResultItemName).ToJson();
        var result = new CraftingRecipeJson(craftingRecipe.Id, ingredients, makes);
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
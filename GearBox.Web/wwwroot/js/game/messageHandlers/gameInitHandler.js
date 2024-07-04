import { CraftingRecipe, CraftingRecipeDeserializer } from "../model/crafting.js";
import { deserializeItemTypeJson, ItemDeserializer, ItemTypeRepository } from "../model/item.js";

// rename & move once game.js is changed
export class GameData {
    #playerId;
    #itemTypes;
    #craftingRecipes;

    /**
     * @param {string} playerId 
     * @param {ItemTypeRepository} itemTypes 
     * @param {CraftingRecipe[]} craftingRecipes 
     */
    constructor(playerId, itemTypes, craftingRecipes) {
        this.#playerId = playerId;
        this.#itemTypes = itemTypes;
        this.#craftingRecipes = craftingRecipes;
    }

    get playerId() { return this.#playerId; }
    get itemTypes() { return this.#itemTypes; }
    get craftingRecipes() { return this.#craftingRecipes; }
}

/**
 * @param {object} json see GameInitJson.cs
 * @returns {GameData}
 */
export function handleGameInit(json) {
    const playerId = json.playerId;
    
    const itemTypes = json.itemTypes.map(deserializeItemTypeJson);
    const itemTypeRepository = new ItemTypeRepository(itemTypes);
    const itemDeserializer = new ItemDeserializer(itemTypeRepository);
    
    const craftingRecipeDeserializer = new CraftingRecipeDeserializer(itemDeserializer);
    const craftingRecipes = json.craftingRecipes.map(x => craftingRecipeDeserializer.deserialize(x));
    
    const result = new GameData(playerId, itemTypeRepository, craftingRecipes);
    return result;
}
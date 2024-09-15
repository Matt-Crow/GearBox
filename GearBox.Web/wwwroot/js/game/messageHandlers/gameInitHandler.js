import { CraftingRecipe, CraftingRecipeDeserializer } from "../model/crafting.js";

// rename & move once game.js is changed
export class GameData {
    #playerId;
    #craftingRecipes;

    /**
     * @param {string} playerId 
     * @param {CraftingRecipe[]} craftingRecipes 
     */
    constructor(playerId, craftingRecipes) {
        this.#playerId = playerId;
        this.#craftingRecipes = craftingRecipes;
    }

    get playerId() { return this.#playerId; }
    get craftingRecipes() { return this.#craftingRecipes; }
}

/**
 * @param {object} json see GameInitJson.cs
 * @returns {GameData}
 */
export function handleGameInit(json) {
    const playerId = json.playerId;
    
    const craftingRecipeDeserializer = new CraftingRecipeDeserializer();
    const craftingRecipes = json.craftingRecipes.map(x => craftingRecipeDeserializer.deserialize(x));
    
    const result = new GameData(playerId, craftingRecipes);
    return result;
}
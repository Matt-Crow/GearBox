import { Item, ItemDeserializer } from "./item.js";


export class CraftingRecipe {
    #ingredients;
    #makes;

    /**
     * @param {Item[]} ingredients 
     * @param {Item} makes 
     */
    constructor(ingredients, makes) {
        this.#ingredients = ingredients;
        this.#makes = makes;
    }

    get ingredients() { return this.#ingredients; }
    get makes() { return this.#makes; }
}

export class CraftingRecipeDeserializer {
    #itemDeserializer;

    /**
     * @param {ItemDeserializer} itemDeserializer 
     */
    constructor(itemDeserializer) {
        this.#itemDeserializer = itemDeserializer;
    }

    deserialize(json) {
        const ingredients = json.ingredients.map(x => this.#itemDeserializer.deserialize(x));
        const makes = this.#itemDeserializer.deserialize(json.makes);
        const result = new CraftingRecipe(ingredients, makes);
        return result;
    }
}

export class CraftingRecipeRepository {
    #recipes;

    /**
     * @param {CraftingRecipe[]} recipes 
     */
    constructor(recipes=[]) {
        this.#recipes = recipes;
    }
}
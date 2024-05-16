import { Item, ItemDeserializer } from "./item.js";


export class CraftingRecipe {
    #id;
    #ingredients;
    #makes;

    /**
     * @param {string} id 
     * @param {Item[]} ingredients 
     * @param {Item} makes 
     */
    constructor(id, ingredients, makes) {
        this.#id = id;
        this.#ingredients = ingredients;
        this.#makes = makes;
    }

    get id() { return this.#id; }
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
        const result = new CraftingRecipe(json.id, ingredients, makes);
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

    get recipes() { return this.#recipes; }
}
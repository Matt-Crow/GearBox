import { Item } from "./item.js";


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
    deserialize(json) {
        const ingredients = json.ingredients.map(x => Item.fromJson(x));
        const makes = Item.fromJson(json.makes);
        const result = new CraftingRecipe(json.id, ingredients, makes);
        return result;
    }
}
import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";
import { CraftingRecipe, CraftingRecipeDeserializer, CraftingRecipeRepository } from "./crafting.js";
import { InventoryDeserializer, ItemDeserializer, ItemTypeRepository, deserializeItemTypeJson } from "./item.js";
import { WorldMap, deserializeMapJson } from "./map.js";
import { Player } from "./player.js";

export class World {
    #playerId; // need reference to changing player
    #map;
    #gameObjects;
    #itemTypes;
    #craftingRecipes;

    /**
     * 
     * @param {string} playerId 
     * @param {WorldMap} map 
     * @param {ItemType[]} itemTypes 
     * @param {CraftingRecipe[]} craftingRecipes 
     */
    constructor(playerId, map, itemTypes, craftingRecipes) {
        this.#playerId = playerId;
        this.#map = map;
        this.#gameObjects = [];
        this.#itemTypes = new ItemTypeRepository(itemTypes);
        this.#craftingRecipes = new CraftingRecipeRepository(craftingRecipes);
    }

    /**
     * @param {any[]} value
     */
    set gameObjects(value) { this.#gameObjects = value; }

    get playerId() { return this.#playerId; }
    get widthInPixels() { return this.#map.widthInPixels; }
    get heightInPixels() { return this.#map.heightInPixels; }
    get itemTypes() { return this.#itemTypes; }
    get craftingRecipes() { return this.#craftingRecipes; }

    /**
     * @returns {Player} the player the client controls
     */
    get player() {
        return this.#gameObjects.find(obj => obj.id == this.#playerId);
    }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        this.#map.draw(context);
        this.#gameObjects.forEach(obj => obj.draw(context));
    }
}

export class WorldInitHandler {

    /**
     * Deserializes and returns the world init message sent by the server
     * @param {object} obj 
     * @returns {World}
     */
    handleWorldInit(obj) {
        const map = deserializeMapJson(obj.map);
        const itemTypes = obj.itemTypes.map(deserializeItemTypeJson);

        // need item types to resolve crafting recipes, dependency injection won't work
        const itemTypeRepository = new ItemTypeRepository(itemTypes);
        const itemDeserializer = new ItemDeserializer(itemTypeRepository);
        const craftingRecipeDeserializer = new CraftingRecipeDeserializer(itemDeserializer);

        const craftingRecipes = obj.craftingRecipes.map(x => craftingRecipeDeserializer.deserialize(x));
        const deserialized = new World(obj.playerId, map, itemTypes, craftingRecipes);
        return deserialized;
    }
}

export class WorldUpdateHandler {
    #world;
    #inventoryDeserializer;
    #itemDeserializer;
    #deserializers = new JsonDeserializers();
    #updateListeners = [];
    #inventoryChangeListeners = [];
    #weaponChangeListeners = [];
    #armorChangeListeners = [];

    /**
     * @param {World} world 
     * @param {ItemDeserializer} itemDeserializer 
     */
    constructor(world, itemDeserializer) {
        this.#world = world;
        this.#inventoryDeserializer = new InventoryDeserializer(itemDeserializer);
        this.#itemDeserializer = itemDeserializer;
    }

    /**
     * @param {JsonDeserializer} deserializer used to deserialize dynamic game objects
     * @returns {WorldUpdateHandler} this, for chaining
     */
    addGameObjectType(deserializer) {
        this.#deserializers.addDeserializer(deserializer);
        return this;
    }

    /**
     * @param {(World) => any} updateListener 
     * @returns {WorldUpdateHandler}
     */
    addUpdateListener(updateListener) {
        this.#updateListeners.push(updateListener);
        return this;
    }

    /**
     * @param {(Inventory) => any} changeListener 
     * @returns {WorldUpdateHandler}
     */
    addInventoryChangeListener(changeListener) {
        this.#inventoryChangeListeners.push(changeListener);
        return this;
    }

    /**
     * @param {(Item?) => any} changeListener 
     * @returns {WorldUpdateHandler}
     */
    addWeaponChangeListener(changeListener) {
        this.#weaponChangeListeners.push(changeListener);
        return this;
    }

    /**
     * @param {(Item?) => any} changeListener 
     * @returns {WorldUpdateHandler}
     */
    addArmorChangeListener(changeListener) {
        this.#armorChangeListeners.push(changeListener);
        return this;
    }

    handleWorldUpdate(json) {
        const newGameObject = json.gameObjects
            .map(gameObjectJson => this.#deserialize(gameObjectJson))
            .filter(obj => obj !== null);
        this.#world.gameObjects = newGameObject;
        
        this.#updateListeners.forEach(listener => listener(this.#world));

        if (json.inventory.hasChanged) {
            const inventory = this.#inventoryDeserializer.deserialize(JSON.parse(json.inventory.body));
            this.#inventoryChangeListeners.forEach(listener => listener(inventory));
        }

        if (json.weapon.hasChanged) {
            const maybeWeapon = JSON.parse(json.weapon.body);
            const weapon = maybeWeapon
                ? this.#itemDeserializer.deserialize(maybeWeapon)
                : null;
            this.#weaponChangeListeners.forEach(listener => listener(weapon));
        }

        if (json.armor.hasChanged) {
            const maybeArmor = JSON.parse(json.armor.body);
            const armor = maybeArmor
                ? this.#itemDeserializer.deserialize(maybeArmor)
                : null;
            this.#armorChangeListeners.forEach(listener => listener(armor));
        }
    }

    #deserialize(gameObjectJson) {
        /*
            gameObjectJson is formatted as
            {
                "type": string
                "content": string (stringified JSON)
            }
        */
        const contentJson = JSON.parse(gameObjectJson.content);
        contentJson["$type"] = gameObjectJson.type;
        return this.#deserializers.deserialize(contentJson);
    }
}

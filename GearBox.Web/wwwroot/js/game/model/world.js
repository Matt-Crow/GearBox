import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";
import { CraftingRecipe, CraftingRecipeDeserializer, CraftingRecipeRepository } from "./crafting.js";
import { InventoryDeserializer, ItemDeserializer, ItemTypeRepository, deserializeItemTypeJson } from "./item.js";
import { TileMap } from "./map.js";
import { Player, PlayerStatSummary } from "./player.js";

export class World {
    #playerId; // need reference to changing player
    #map;
    #gameObjects;
    #itemTypes;
    #craftingRecipes;

    /**
     * 
     * @param {string} playerId 
     * @param {TileMap} map 
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
        this.#map.drawPitsAndFloor(context);
        this.#gameObjects.forEach(obj => obj.draw(context));
        this.#map.drawWalls(context);
    }
}

export class AreaInitHandler {

    /**
     * @param {object} obj 
     * @returns {World}
     */
    handleAreaInit(obj) {
        const map = TileMap.fromJson(obj.map);
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

export class AreaUpdateHandler {
    #world;
    #inventoryDeserializer;
    #itemDeserializer;
    #deserializers = new JsonDeserializers();
    #updateListeners = [];
    #inventoryChangeListeners = [];
    #weaponChangeListeners = [];
    #armorChangeListeners = [];
    #statSummaryChangeListeners = [];

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
     * @returns {AreaUpdateHandler} this, for chaining
     */
    addGameObjectType(deserializer) {
        this.#deserializers.addDeserializer(deserializer);
        return this;
    }

    /**
     * @param {(World) => any} updateListener 
     * @returns {AreaUpdateHandler}
     */
    addUpdateListener(updateListener) {
        this.#updateListeners.push(updateListener);
        return this;
    }

    /**
     * @param {(Inventory) => any} changeListener 
     * @returns {AreaUpdateHandler}
     */
    addInventoryChangeListener(changeListener) {
        this.#inventoryChangeListeners.push(changeListener);
        return this;
    }

    /**
     * @param {(Item?) => any} changeListener 
     * @returns {AreaUpdateHandler}
     */
    addWeaponChangeListener(changeListener) {
        this.#weaponChangeListeners.push(changeListener);
        return this;
    }

    /**
     * @param {(Item?) => any} changeListener 
     * @returns {AreaUpdateHandler}
     */
    addArmorChangeListener(changeListener) {
        this.#armorChangeListeners.push(changeListener);
        return this;
    }

    /**
     * @param {(PlayerStatSummary) => any} changeListener 
     * @returns {AreaUpdateHandler}
     */
    addStatSummaryChangeListener(changeListener) {
        this.#statSummaryChangeListeners.push(changeListener);
        return this;
    }

    handleAreaUpdate(json) {
        const newGameObject = json.gameObjects
            .map(gameObjectJson => this.#deserialize(gameObjectJson))
            .filter(obj => obj !== null);
        this.#world.gameObjects = newGameObject;
        
        this.#updateListeners.forEach(listener => listener(this.#world));

        this.#handleChanges(json.changes.inventory, v => this.#inventoryDeserializer.deserialize(v), this.#inventoryChangeListeners);
        this.#handleChanges(json.changes.weapon, v => this.#itemDeserializer.deserialize(v), this.#weaponChangeListeners);
        this.#handleChanges(json.changes.armor, v => this.#itemDeserializer.deserialize(v), this.#armorChangeListeners);
        this.#handleChanges(json.changes.summary, PlayerStatSummary.fromJson, this.#statSummaryChangeListeners);
    }

    #handleChanges(maybeChange, deserialize, listeners) {
        if (maybeChange.hasChanged) {
            const value = deserialize(maybeChange.value);
            listeners.forEach(listener => listener(value));
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

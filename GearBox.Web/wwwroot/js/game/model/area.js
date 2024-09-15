import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";
import { GameData } from "../messageHandlers/gameInitHandler.js";
import { InventoryDeserializer, Item } from "./item.js";
import { TileMap } from "./map.js";
import { Player, PlayerStatSummary } from "./player.js";
import { OpenShopDeserializer, Shop } from "./shop.js";

export class Area {
    #gameData;
    #map;
    #gameObjects = [];
    #shops = [];

    /**
     * @param {GameData} gameData 
     * @param {TileMap} map 
     * @param {Shop[]} shops 
     */
    constructor(gameData, map, shops) {
        this.#gameData = gameData;
        this.#map = map;
        this.#shops = shops;
    }

    /**
     * @param {any[]} value
     */
    set gameObjects(value) { this.#gameObjects = value; }

    get widthInPixels() { return this.#map.widthInPixels; }
    get heightInPixels() { return this.#map.heightInPixels; }

    /**
     * @returns {Player} the player the client controls
     */
    get player() {
        return this.#gameObjects.find(obj => obj.id == this.#gameData.playerId);
    }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        this.#map.drawPitsAndFloor(context);
        this.#gameObjects.forEach(obj => obj.draw(context));
        this.#shops.forEach(shop => shop.draw(context));
        this.#map.drawWalls(context);
    }
}

export class AreaUpdateHandler {
    #area;
    #inventoryDeserializer;
    #openShopDeserializer;
    #deserializers = new JsonDeserializers();
    #updateListeners = [];
    #inventoryChangeListeners = [];
    #weaponChangeListeners = [];
    #armorChangeListeners = [];
    #statSummaryChangeListeners = [];
    #openShopChangeListeners = [];

    /**
     * @param {Area} area 
     */
    constructor(area) {
        this.#area = area;
        this.#inventoryDeserializer = new InventoryDeserializer();
        this.#openShopDeserializer = new OpenShopDeserializer();
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
     * @param {(Area) => any} updateListener 
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

    /**
     * @param {(OpenShop) => any} changeListener 
     * @returns {AreaUpdateHandler}
     */
    addOpenShopChangeListener(changeListener) {
        this.#openShopChangeListeners.push(changeListener);
        return this;
    }

    handleAreaUpdate(json) {
        const newGameObject = json.gameObjects
            .map(gameObjectJson => this.#deserialize(gameObjectJson))
            .filter(obj => obj !== null);
        this.#area.gameObjects = newGameObject;
        
        this.#updateListeners.forEach(listener => listener(this.#area));

        this.#handleChanges(json.uiStateChanges.inventory, v => this.#inventoryDeserializer.deserialize(v), this.#inventoryChangeListeners);
        this.#handleChanges(json.uiStateChanges.weapon, v => Item.fromJson(v), this.#weaponChangeListeners);
        this.#handleChanges(json.uiStateChanges.armor, v => Item.fromJson(v), this.#armorChangeListeners);
        this.#handleChanges(json.uiStateChanges.summary, PlayerStatSummary.fromJson, this.#statSummaryChangeListeners);
        this.#handleChanges(json.uiStateChanges.openShop, v => this.#openShopDeserializer.deserialize(v), this.#openShopChangeListeners);
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

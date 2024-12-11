import { Views } from "../components/views/views.js";
import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";
import { GameData } from "../messageHandlers/gameInitHandler.js";
import { ActiveAbility } from "./activeAbility.js";
import { MaybeChange, UiStateChanges } from "./areaUpdate.js";
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
    #views;
    #inventoryDeserializer;
    #openShopDeserializer;
    #deserializers = new JsonDeserializers();
    #updateListeners = [];

    /**
     * @param {Area} area 
     * @param {Views} views 
     */
    constructor(area, views) {
        this.#views = views;
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

    handleAreaUpdate(json) {
        const newGameObject = json.gameObjects
            .map(gameObjectJson => this.#deserialize(gameObjectJson))
            .filter(obj => obj !== null);
        this.#area.gameObjects = newGameObject;
        
        this.#updateListeners.forEach(listener => listener(this.#area));
        
        const uiStateChanges = new UiStateChanges(
            null, // todo area
            MaybeChange.fromJson(json.uiStateChanges.inventory, v => this.#inventoryDeserializer.deserialize(v)),
            MaybeChange.fromJson(json.uiStateChanges.weapon, Item.fromJson),
            MaybeChange.fromJson(json.uiStateChanges.armor, Item.fromJson),
            MaybeChange.fromJson(json.uiStateChanges.summary, PlayerStatSummary.fromJson),
            MaybeChange.fromJson(json.uiStateChanges.actives, actives => actives.map(ActiveAbility.fromJson)),
            MaybeChange.fromJson(json.uiStateChanges.openShop, v => this.#openShopDeserializer.deserialize(v))
        );
        this.#views.handleUiStateChanges(uiStateChanges);
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

import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";
import { ItemTypeRepository, deserializeItemTypeJson } from "./item.js";
import { WorldMap, deserializeMapJson } from "./map.js";
import { Player } from "./player.js";

export class World {
    #playerId; // need reference to changing player
    #map;
    #gameObjects;
    #itemTypes;

    /**
     * 
     * @param {string} playerId 
     * @param {WorldMap} map 
     * @param {ItemTypeRepository} itemTypes 
     */
    constructor(playerId, map, itemTypes) {
        this.#playerId = playerId;
        this.#map = map;
        this.#gameObjects = [];
        this.#itemTypes = new ItemTypeRepository(itemTypes);
    }

    /**
     * @param {any[]} value
     */
    set gameObjects(value) { this.#gameObjects = value; }

    get playerId() { return this.#playerId; }
    get widthInPixels() { return this.#map.widthInPixels; }
    get heightInPixels() { return this.#map.heightInPixels; }
    get itemTypes() { return this.#itemTypes; }

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
        const deserialized = new World(obj.playerId, map, itemTypes);
        return deserialized;
    }
}

export class WorldUpdateHandler {
    #world;
    #deserializers = new JsonDeserializers();
    #updateListeners = [];

    /**
     * @param {World} world 
     */
    constructor(world) {
        this.#world = world;
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

    handleWorldUpdate(obj) {
        const newGameObject = obj.gameObjects
            .map(gameObjectJson => this.#deserialize(gameObjectJson))
            .filter(obj => obj !== null);
        this.#world.gameObjects = newGameObject;
        this.#updateListeners.forEach(listener => listener(this.#world));
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

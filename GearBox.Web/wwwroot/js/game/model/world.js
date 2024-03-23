import { Change, ChangeHandlers } from "../infrastructure/change.js";
import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";
import { Character } from "./character.js";
import { ItemTypeRepository, deserializeItemTypeJson } from "./item.js";
import { deserializeMapJson } from "./map.js";

export class World {
    #playerId; // need reference to changing player
    #map;
    #dynamicGameObjects;
    #stableGameObjects;
    #itemTypes;

    constructor(playerId, map, itemTypes) {
        this.#playerId = playerId;
        this.#map = map;
        this.#dynamicGameObjects = [];
        this.#stableGameObjects = new Map();
        this.#itemTypes = new ItemTypeRepository(itemTypes);
    }

    /**
     * @param {any[]} value
     */
    set dynamicGameObjects(value) {
        this.#dynamicGameObjects = value;
    }

    /**
     * @returns {Character} the player the client controls
     */
    get player() {
        return this.#dynamicGameObjects.find(obj => obj.id == this.#playerId);
    }

    /**
     * @returns {string}
     */
    get playerId() {
        return this.#playerId;
    }

    /**
     * @returns {number}
     */
    get widthInPixels() {
        return this.#map.widthInPixels;
    }

    /**
     * @returns {number}
     */
    get heightInPixels() {
        return this.#map.heightInPixels;
    }

    get itemTypes() {
        return this.#itemTypes;
    }

    saveStableGameObject(obj) {
        this.#stableGameObjects.set(obj.id, obj);
    }

    removeStableGameObject(id) {
        this.#stableGameObjects.delete(id);
    }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        this.#map.draw(context);
        this.#dynamicGameObjects.forEach(obj => obj.draw(context));
        for (const obj of this.#stableGameObjects.values()) {
            obj.draw(context);
        }
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
    #changeHandlers;
    #deserializers;

    /**
     * @param {World} world 
     * @param {ChangeHandlers} changeHandlers 
     */
    constructor(world, changeHandlers) {
        this.#world = world;
        this.#changeHandlers = changeHandlers;
        this.#deserializers = new JsonDeserializers();
    }

    /**
     * @param {JsonDeserializer} deserializer used to deserialize dynamic game objects
     * @returns {WorldUpdateHandler} this, for chaining
     */
    withDynamicObjectDeserializer(deserializer) {
        this.#deserializers.addDeserializer(deserializer);
        return this;
    }

    handleWorldUpdate(obj) {
        const dynamicGameObjects = obj.dynamicWorldContent.gameObjects.map(x => this.#deserializers.deserialize(x));
        this.#world.dynamicGameObjects = dynamicGameObjects;
        
        const changes = obj.changes.map(json => Change.fromJson(json));
        changes.forEach(change => this.#changeHandlers.handle(change));
    }
}
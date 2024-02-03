import { Change, ChangeHandlers } from "../infrastructure/change.js";
import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";
import { MessageHandler } from "../infrastructure/messageHandler.js";
import { Character } from "./character.js";
import { InventoryItemTypeRepository, deserializeItemTypeJson } from "./item.js";
import { deserializeMapJson } from "./map.js";

export class WorldProxy {
    #world;

    constructor() {
        this.#world = null;
    }

    /**
     * @returns {World}
     */
    get value() {
        if (this.#world === null) {
            throw new Error("world has not been set yet");
        }
        return this.#world;
    }

    /**
     * @param {World} world 
     */
    set value(world) {
        this.#world = world;
    }
}

export class World {
    #playerId; // need reference to changing player
    #map;
    #staticGameObjects;
    #dynamicGameObjects;
    #itemTypes;

    constructor(playerId, map, staticGameObjects, itemTypes) {
        this.#playerId = playerId;
        this.#map = map;
        this.#staticGameObjects = staticGameObjects;
        this.#dynamicGameObjects = [];
        this.#itemTypes = new InventoryItemTypeRepository(itemTypes);
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

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        this.#map.draw(context);
        this.#staticGameObjects.forEach(obj => obj.draw(context));
        this.#dynamicGameObjects.forEach(obj => obj.draw(context));
    }
}

// unrelated to JSON deserializers, as those require type
export class WorldDeserializer {
    
    #changeHandlers;
    #dynamicObjectDeserializers;

    /**
     * @param {ChangeHandlers} changeHandlers 
     */
    constructor(changeHandlers) {
        this.#changeHandlers = changeHandlers;
        this.#dynamicObjectDeserializers = new JsonDeserializers();
    }

    /**
     * @param {JsonDeserializer} deserializer used to deserialize dynamic game objects
     */
    addDynamicObjectDeserializer(deserializer) {
        this.#dynamicObjectDeserializers.addDeserializer(deserializer);
    }

    deserializeWorldInitBody(obj) {
        const map = deserializeMapJson(obj.staticWorldContent.map);
        const staticGameObjects = this.#deserializeStaticGameObjects(obj.staticWorldContent.gameObjects);
        const itemTypes = obj.itemTypes.map(deserializeItemTypeJson);
        return new World(obj.playerId, map, staticGameObjects, itemTypes);
    }

    deserializeWorldUpdateBody(obj) {
        const dynamicGameObjects = obj.dynamicWorldContent.gameObjects.map(x => this.#dynamicObjectDeserializers.deserialize(x));
        const changes = obj.changes.map(json => Change.fromJson(json));
        changes.forEach(change => this.#changeHandlers.handle(change));
        return dynamicGameObjects;
    }

    #deserializeStaticGameObjects(gameObjectsJson) {
        if (gameObjectsJson.length > 0) {
            throw new Error("not implemented yet");
        }
        return [];
    }
}

export class WorldInitHandler extends MessageHandler {
    constructor(worldProxy, worldDeserializer, callback) {
        super("WorldInit", (obj) => {
            const deserialized = worldDeserializer.deserializeWorldInitBody(obj);
            worldProxy.value = deserialized;
            callback();
        });
    }
}

export class WorldUpdateHandler extends MessageHandler {
    
    constructor(worldProxy, worldDeserializer) {
        super("WorldUpdate", (obj) => {
            const deserialized = worldDeserializer.deserializeWorldUpdateBody(obj);
            worldProxy.value.dynamicGameObjects = deserialized;
        });
    }
}
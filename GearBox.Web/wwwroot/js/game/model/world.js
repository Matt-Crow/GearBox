import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";
import { MessageHandler } from "../infrastructure/messageHandler.js";
import { Map as WorldMap } from "./map.js";

export class WorldProxy {
    #world;

    constructor() {
        this.#world = null;
    }

    get value() {
        if (this.#world === null) {
            throw new Error("world has not been set yet");
        }
        return this.#world;
    }

    set value(world) {
        this.#world = world;
    }
}

export class World {
    #map;
    #staticGameObjects;
    #dynamicGameObjects;

    constructor(map, staticGameObjects) {
        this.#map = map;
        this.#staticGameObjects = staticGameObjects;
        this.#dynamicGameObjects = [];
    }

    /**
     * @param {any[]} value
     */
    set dynamicGameObjects(value) {
        this.#dynamicGameObjects = value;
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
    
    #dynamicObjectDeserializers;

    constructor() {
        this.#dynamicObjectDeserializers = new JsonDeserializers();
    }

    /**
     * @param {JsonDeserializer} deserializer used to deserialize dynamic game objects
     */
    addDynamicObjectDeserializer(deserializer) {
        this.#dynamicObjectDeserializers.addDeserializer(deserializer);
    }

    deserializeWorldInitBody(obj) {
        const map = this.#deserializeMap(obj.map);
        const staticGameObjects = this.#deserializeStaticGameObjects(obj.gameObjects);
        return new World(map, staticGameObjects);
    }

    deserializeWorldUpdateBody(obj) {
        const dynamicGameObjects = obj.gameObjects.map(x => this.#dynamicObjectDeserializers.deserialize(x));
        return dynamicGameObjects;
    }

    #deserializeMap(obj) {
        const tileTypes = new Map();
        obj.tileTypes.forEach(tileType => {
            const r = tileType.value.color.red;
            const g = tileType.value.color.green;
            const b = tileType.value.color.blue;
            const color = `rgb(${r} ${g} ${b})`; // CSS color doesn't use commas
            // https://developer.mozilla.org/en-US/docs/Web/CSS/color_value/rgb

            tileTypes.set(tileType.key, {
                color: color,
                isTangible: tileType.value.isTangible
            });
        });

        return new WorldMap({
            tileMap: obj.tileMap, 
            tileTypes: tileTypes
        });
    }

    #deserializeStaticGameObjects(gameObjectsJson) {
        if (gameObjectsJson.length > 0) {
            throw new Error("not implemented yet");
        }
        return [];
    }
}

export class WorldInitHandler extends MessageHandler {
    constructor(worldProxy, worldDeserializer) {
        super("WorldInit", (obj) => {
            const deserialized = worldDeserializer.deserializeWorldInitBody(obj);
            worldProxy.value = deserialized;
        });
    }
}

export class WorldUpdateHandler extends MessageHandler {
    
    constructor(worldProxy, jsonDeserializers) {
        super("WorldUpdate", (obj) => {
            const deserialized = obj.gameObjects.map(json => jsonDeserializers.deserialize(json));
            worldProxy.value.dynamicGameObjects = deserialized;
        });
    }
}
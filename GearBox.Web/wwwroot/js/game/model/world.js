import { MessageHandler } from "../infrastructure/messageHandler.js";

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
    #dynamicGameObjects;

    constructor() {
        this.#dynamicGameObjects = [];
    }

    /**
     * @param {any[]} value
     */
    set dynamicGameObjects(value) {
        this.#dynamicGameObjects = value;
    }
}

export class WorldInitHandler extends MessageHandler {
    constructor(worldProxy, jsonDeserializers) {
        super("WorldInit", (obj) => {
            //TODO no deserializers registered yet - errors occur, but don't show in console
            const deserialized = jsonDeserializers.deserialize(obj);
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
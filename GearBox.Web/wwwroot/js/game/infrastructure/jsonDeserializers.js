import { JsonDeserializer } from "./jsonDeserializer.js";

export class JsonDeserializers {
    #deserializers;

    constructor() {
        this.#deserializers = new Map();
    }

    /**
     * @param {JsonDeserializer} jsonDeserializer 
     */
    addDeserializer(jsonDeserializer) {
        this.#deserializers.set(jsonDeserializer.type, jsonDeserializer);
    }

    /**
     * @param {object} obj 
     * @returns {object}
     */
    deserialize(obj) {
        const type = obj["$type"];
        if (!this.#deserializers.has(type)) {
            throw new Error(`Unsupported JSON value for $type: ${type}`);
        }
        return this.#deserializers.get(type).deserialize(obj);
    }
}
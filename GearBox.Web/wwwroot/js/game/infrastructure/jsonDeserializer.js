export class JsonDeserializer {
    #type;
    #deserializer;

    /**
     * @param {string} type 
     * @param {(object) => object} deserializer 
     */
    constructor(type, deserializer) {
        this.#type = type;
        this.#deserializer = deserializer;
    }

    get type() {
        return this.#type;
    }

    deserialize(obj) {
        return this.#deserializer(obj);
    }
}
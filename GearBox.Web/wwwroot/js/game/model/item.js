import { ChangeHandler } from "../infrastructure/change.js";
import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";

export class ItemChangeHandler extends ChangeHandler {
    #deserializers;
    
    constructor() {
        super("inventoryItem");
        this.#deserializers = new JsonDeserializers();
    }

    /**
     * @param {JsonDeserializer} jsonDeserializer a deserializer for a specific 
     *  item type
     * @returns {ItemChangeHandler}
     */
    withDeserializer(jsonDeserializer) {
        this.#deserializers.addDeserializer(jsonDeserializer);
        return this;
    }

    handleCreate(body) {
        console.log(`Create item ${body}`);
    }

    handleUpdate(body) {
        console.log(`Update item ${body}`);
    }

    handleDelete(body) {
        console.log(`Delete item ${body}`);
    }
}
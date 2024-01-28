import { ChangeHandler } from "../infrastructure/change.js";
import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { JsonDeserializers } from "../infrastructure/jsonDeserializers.js";
import { TestCase, TestSuite } from "../testing/tests.js";

export class InventoryItemType {
    #name;
    #isStackable;

    constructor(name, isStackable) {
        this.#name = name;
        this.#isStackable = isStackable;
    }

    get name() {
        return this.#name;
    }

    get isStackable() {
        return this.#isStackable;
    }
}

export function deserializeItemTypeJson(obj) {
    const result = new InventoryItemType(obj.name, obj.isStackable);
    return result;
}

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

export class InventoryItemTypeRepository {
    #itemTypes = new Map();

    /**
     * @param {InventoryItemType[]} itemTypes
     */
    constructor(itemTypes=[]) {
        itemTypes.forEach(x => this.#itemTypes.set(x.name, x));
    }

    /**
     * @param {string} name 
     * @returns {InventoryItemType}
     */
    getItemTypeByName(name) {
        const result = this.#itemTypes.has(name)
            ? this.#itemTypes.get(name)
            : null;
        return result;
    }
}

export const itemTests = new TestSuite("item.js", [
    new TestCase("getItemTypeByName_givenNotFound_returnsNull", (assert) => {
        var sut = new InventoryItemTypeRepository();
        assert.isNull(sut.getItemTypeByName("foo"));
    }),
    new TestCase("getItemTypeByName_givenFound_returnsIt", (assert) => {
        var expected = new InventoryItemType("foo", true);
        var sut = new InventoryItemTypeRepository([expected]);

        var actual = sut.getItemTypeByName(expected.name);

        assert.equal(expected, actual);
    })
]);
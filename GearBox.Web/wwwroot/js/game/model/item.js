import { TestCase, TestSuite } from "../testing/tests.js";

export class Inventory {
    #equipment;
    #materials;

    /**
     * @param {Item[]} equipment 
     * @param {Item[]} materials 
     */
    constructor(equipment=[], materials=[]) {
        this.#equipment = equipment;
        this.#materials = materials;
    }

    get equipment() {
        return this.#equipment;
    }

    get materials() {
        return this.#materials;
    }
}

export class InventoryDeserializer {
    #itemDeserializer;

    /**
     * @param {ItemDeserializer} itemDeserializer 
     */
    constructor(itemDeserializer) {
        this.#itemDeserializer = itemDeserializer;
    }

    deserialize(json) {
        const equipment = json.equipment.items.map(x => this.#itemDeserializer.deserilize(x));
        const materials = json.materials.items.map(x => this.#itemDeserializer.deserilize(x));
        return new Inventory(equipment, materials);
    }
}

export class Item {
    #type;
    #description;
    #metadata;
    #tags;
    #quantity;

    /**
     * @param {ItemType} type 
     * @param {string} description 
     * @param {Map<string, object?>} metadata
     * @param {string[]} tags
     * @param {number} quantity 
     */
    constructor(type, description, metadata, tags, quantity) {
        this.#type = type;
        this.#description = description;
        this.#metadata = metadata;
        this.#tags = tags;
        this.#quantity = quantity;
    }

    get type() {
        return this.#type;
    }

    get description() {
        return this.#description;
    }

    get metadata() {
        return this.#metadata;
    }

    get tags() {
        return this.#tags;
    }

    get quantity() {
        return this.#quantity;
    }
}

export class ItemDeserializer {
    #itemTypes;

    /**
     * @param {ItemTypeRepository} itemTypes 
     */
    constructor(itemTypes) {
        this.#itemTypes = itemTypes;
    }

    deserilize(json) {
        const type = this.#itemTypes.getItemTypeByName(json.name);
        if (type === null) {
            throw new Error(`Bad item type name: "${json.name}"`);
        }

        const metadata = new Map();
        for (const datum of json.metadata) {
            metadata.set(datum.key, datum.value);
        }

        const result = new Item(
            type,
            json.description,
            metadata,
            json.tags.slice(),
            json.quantity
        );
        return result;
    }
}

export class ItemType {
    #name;

    constructor(name) {
        this.#name = name;
    }

    get name() {
        return this.#name;
    }
}

export function deserializeItemTypeJson(obj) {
    const result = new ItemType(obj.name);
    return result;
}

export class ItemTypeRepository {
    #itemTypes = new Map();

    /**
     * @param {ItemType[]} itemTypes
     */
    constructor(itemTypes=[]) {
        itemTypes.forEach(x => this.#itemTypes.set(x.name, x));
    }

    /**
     * @param {string} name 
     * @returns {ItemType}
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
        var sut = new ItemTypeRepository();
        assert.isNull(sut.getItemTypeByName("foo"));
    }),
    new TestCase("getItemTypeByName_givenFound_returnsIt", (assert) => {
        var expected = new ItemType("foo", true);
        var sut = new ItemTypeRepository([expected]);

        var actual = sut.getItemTypeByName(expected.name);

        assert.equal(expected, actual);
    })
]);
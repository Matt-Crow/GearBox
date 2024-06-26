import { TestCase, TestSuite } from "../testing/tests.js";

export class Inventory {
    #weapons;
    #armors;
    #materials;

    /**
     * @param {Item[]} weapons 
     * @param {Item[]} armors 
     * @param {Item[]} materials 
     */
    constructor(weapons=[], armors=[], materials=[]) {
        this.#weapons = weapons;
        this.#armors = armors;
        this.#materials = materials;
    }

    get weapons() { return this.#weapons; }
    get armors() { return this.#armors; }
    get materials() { return this.#materials; }
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
        const weapons = json.weapons.items.map(x => this.#itemDeserializer.deserialize(x));
        const armors = json.armors.items.map(x => this.#itemDeserializer.deserialize(x));
        const materials = json.materials.items.map(x => this.#itemDeserializer.deserialize(x));
        return new Inventory(weapons, armors, materials);
    }
}

export class Item {
    #id;
    #type;
    #description;
    #level;
    #details;
    #quantity;

    /**
     * @param {string?} id 
     * @param {ItemType} type 
     * @param {string} description 
     * @param {number} level
     * @param {string[]} details 
     * @param {number} quantity 
     */
    constructor(id, type, description, level, details, quantity) {
        this.#id = id;
        this.#type = type;
        this.#description = description;
        this.#level = level;
        this.#details = details;
        this.#quantity = quantity;
    }

    get id() {
        return this.#id;
    }

    get type() {
        return this.#type;
    }

    get description() {
        return this.#description;
    }

    get level() {
        return this.#level;
    }

    get details() {
        return this.#details;
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

    deserialize(json) {
        if (!json) {
            return null;
        }
        const type = this.#itemTypes.getItemTypeByName(json.name);
        if (type === null) {
            throw new Error(`Bad item type name: "${json.name}"`);
        }

        const result = new Item(
            json.id,
            type,
            json.description,
            json.level,
            json.details.slice(),
            json.quantity
        );
        return result;
    }
}

export class ItemType {
    #name;
    #gradeOrder;
    #gradeName;

    constructor(name, gradeOrder, gradeName) {
        this.#name = name;
        this.#gradeOrder = gradeOrder;
        this.#gradeName = gradeName;
    }

    get name() {
        return this.#name;
    }

    get gradeOrder() {
        return this.#gradeOrder;
    }

    get gradeName() {
        return this.#gradeName;
    }
}

export function deserializeItemTypeJson(obj) {
    const result = new ItemType(obj.name, obj.gradeOrder, obj.gradeName);
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
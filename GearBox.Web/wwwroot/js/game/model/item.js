import { ActiveAbility } from "./activeAbility.js";

export class Inventory {
    #weapons;
    #armors;
    #materials;
    #gold;

    /**
     * @param {Item[]} weapons 
     * @param {Item[]} armors 
     * @param {Item[]} materials 
     * @param {number} gold
     */
    constructor(weapons=[], armors=[], materials=[], gold=0) {
        this.#weapons = weapons;
        this.#armors = armors;
        this.#materials = materials;
        this.#gold = gold;
    }

    get weapons() { return this.#weapons; }
    get armors() { return this.#armors; }
    get materials() { return this.#materials; }
    get gold() { return this.#gold; }
}

export class InventoryDeserializer {
    deserialize(json) {
        const weapons = json.weapons.items.map(x => Item.fromJson(x));
        const armors = json.armors.items.map(x => Item.fromJson(x));
        const materials = json.materials.items.map(x => Item.fromJson(x));
        return new Inventory(weapons, armors, materials, json.gold);
    }
}

export class Item {
    #id;
    #name;
    #gradeName;
    #gradeOrder;
    #description;
    #level;
    #details;
    #quantity;
    #actives;

    /**
     * @param {string?} id
     * @param {string} name 
     * @param {string} gradeName 
     * @param {number} gradeOrder  
     * @param {string} description 
     * @param {number} level
     * @param {string[]} details 
     * @param {number} quantity 
     * @param {ActiveAbility[]} actives 
     */
    constructor(id, name, gradeName, gradeOrder, description, level, details, quantity, actives) {
        this.#id = id;
        this.#name = name;
        this.#gradeName = gradeName;
        this.#gradeOrder = gradeOrder;
        this.#description = description;
        this.#level = level;
        this.#details = details;
        this.#quantity = quantity;
        this.#actives = actives;
    }

    get id() { return this.#id; }
    get name() { return this.#name; }
    get gradeName() { return this.#gradeName; }
    get gradeOrder() { return this.#gradeOrder; }
    get description() { return this.#description; }
    get level() { return this.#level; }
    get details() { return this.#details; }
    get quantity() { return this.#quantity; }
    get actives() { return this.#actives; }

    /**
     * @param {object} json 
     * @returns {Item|null}
     */
    static fromJson(json) {
        if (!json) {
            return null;
        }

        const result = new Item(
            json.id,
            json.name,
            json.gradeName,
            json.gradeOrder,
            json.description,
            json.level,
            json.details.slice(),
            json.quantity,
            json.actives.map(ActiveAbility.fromJson)
        );
        return result;
    }
}
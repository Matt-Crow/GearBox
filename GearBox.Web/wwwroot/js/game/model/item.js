import { ActiveAbility } from "./activeAbility.js";
import { PassiveAbility } from "./passiveAbility.js";

export class Inventory {
    #manipulators;
    #torsos;
    #materials;
    #gold;

    /**
     * @param {Item[]} manipulators 
     * @param {Item[]} torsos 
     * @param {Item[]} materials 
     * @param {number} gold
     */
    constructor(manipulators=[], torsos=[], materials=[], gold=0) {
        this.#manipulators = manipulators;
        this.#torsos = torsos;
        this.#materials = materials;
        this.#gold = gold;
    }

    get manipulators() { return this.#manipulators; }
    get torsos() { return this.#torsos; }
    get materials() { return this.#materials; }
    get gold() { return this.#gold; }
}

export class InventoryDeserializer {
    deserialize(json) {
        const manipulators = json.manipulators.items.map(x => Item.fromJson(x));
        const torsos = json.torsos.items.map(x => Item.fromJson(x));
        const materials = json.materials.items.map(x => Item.fromJson(x));
        return new Inventory(manipulators, torsos, materials, json.gold);
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
    #passives;

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
     * @param {PassiveAbility[]} passives
     */
    constructor(id, name, gradeName, gradeOrder, description, level, details, quantity, actives, passives) {
        this.#id = id;
        this.#name = name;
        this.#gradeName = gradeName;
        this.#gradeOrder = gradeOrder;
        this.#description = description;
        this.#level = level;
        this.#details = details;
        this.#quantity = quantity;
        this.#actives = actives;
        this.#passives = passives;
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
    get passives() { return this.#passives; }

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
            json.actives.map(ActiveAbility.fromJson),
            json.passives.map(PassiveAbility.fromJson)
        );
        return result;
    }
}
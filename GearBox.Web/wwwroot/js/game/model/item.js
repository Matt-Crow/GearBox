import { ActiveAbility } from "./activeAbility.js";
import { PassiveAbility } from "./passiveAbility.js";

export const PART_SLOT_TYPES = {
    HEAD: "Head",
    LOCOMOTION: "Locomotion",
    MANIPULATOR: "Manipulator",
    TORSO: "Torso"
};

export class Inventory {
    #parts;
    #materials;
    #gold;

    /**
     * @param {Item[]} parts 
     * @param {Item[]} materials 
     * @param {number} gold
     */
    constructor(parts=[], materials=[], gold=0) {
        this.#parts = parts;
        this.#materials = materials;
        this.#gold = gold;
    }

    get parts() { return this.#parts; }
    get materials() { return this.#materials; }
    get gold() { return this.#gold; }
}

export class InventoryDeserializer {
    deserialize(json) {
        const parts = json.parts.map(x => Item.fromJson(x));
        const materials = json.materials.items.map(x => Item.fromJson(x));
        return new Inventory(parts, materials, json.gold);
    }
}

export class Item {
    #id;
    #name;
    #gradeName;
    #gradeOrder;
    #description;
    #level;
    #slotType;
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
     * @param {string} slotType
     * @param {string[]} details 
     * @param {number} quantity 
     * @param {ActiveAbility[]} actives 
     * @param {PassiveAbility[]} passives
     */
    constructor(id, name, gradeName, gradeOrder, description, level, slotType, details, quantity, actives, passives) {
        this.#id = id;
        this.#name = name;
        this.#gradeName = gradeName;
        this.#gradeOrder = gradeOrder;
        this.#description = description;
        this.#level = level;
        this.#slotType = slotType;
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
    get slotType() { return this.#slotType; }
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
            json.slotType,
            json.details.slice(),
            json.quantity,
            json.actives.map(ActiveAbility.fromJson),
            json.passives.map(PassiveAbility.fromJson)
        );
        return result;
    }
}
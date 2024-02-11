import { ChangeHandler } from "../infrastructure/change.js";
import { PIXELS_PER_TILE } from "./constants.js";
import { World } from "./world.js";

export class LootChestChangeHandler extends ChangeHandler {
    #world;

    /**
     * @param {World} world 
     */
    constructor(world) {
        super("lootChest");
        this.#world = world;
    }

    handleContent(obj) {
        const lootChest = this.#deserialize(obj);
        if (lootChest.hasBeenCollectedBy(this.#world.playerId)) {
            this.#world.removeStableGameObject(lootChest.id);
        } else {
            this.#world.saveStableGameObject(lootChest);
        }
    }

    handleDelete(obj) {
        this.#world.removeStableGameObject(obj.id);
    }

    #deserialize(json) {
        const result = new LootChest(json.id, json.x, json.y, json.collectedBy);
        return result;
    }
}

export class LootChest {
    #id;
    #x;
    #y;
    #collectedBy;

    /**
     * @param {string} id 
     * @param {number} x 
     * @param {number} y 
     * @param {string[]} collectedBy 
     */
    constructor(id, x, y, collectedBy) {
        this.#id = id;
        this.#x = x;
        this.#y = y;
        this.#collectedBy = collectedBy;
    }

    /**
     * @returns {string}
     */
    get id() {
        return this.#id;
    }

    /**
     * @param {string} playerId 
     * @returns {boolean}
     */
    hasBeenCollectedBy(playerId) {
        return this.#collectedBy.includes(playerId);
    }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        const small = PIXELS_PER_TILE / 20;
        context.fillStyle = "brown";
        context.fillRect(
            this.#x + small, 
            this.#y + small, 
            PIXELS_PER_TILE - small*2, 
            PIXELS_PER_TILE - small*2
        );
    }
}
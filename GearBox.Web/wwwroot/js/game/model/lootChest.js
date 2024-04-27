import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { PIXELS_PER_TILE } from "./constants.js";

export class LootChestJsonDeserializer extends JsonDeserializer {
    #playerId;
    
    constructor(playerId) {
        super("lootChest", obj => this.#handle(obj));
        this.#playerId = playerId;
    }

    #handle(json) {
        const chest = new LootChest(json.x, json.y, json.collectedBy);
        const result = chest.hasBeenCollectedBy(this.#playerId) 
            ? null 
            : chest;
        return result;
    }
}

export class LootChest {
    #x;
    #y;
    #collectedBy;

    /**
     * @param {number} x center of the LootChest
     * @param {number} y center of the LootChest
     * @param {string[]} collectedBy 
     */
    constructor(x, y, collectedBy) {
        this.#x = x;
        this.#y = y;
        this.#collectedBy = collectedBy;
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
        const radius = PIXELS_PER_TILE / 2;
        const small = PIXELS_PER_TILE / 20;
        context.fillStyle = "brown";
        context.fillRect(
            this.#x - radius + small, 
            this.#y - radius + small, 
            PIXELS_PER_TILE - small*2, 
            PIXELS_PER_TILE - small*2
        );
    }
}
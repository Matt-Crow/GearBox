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
        context.fillStyle = "brown";
        
        // draw pentagon
        // https://stackoverflow.com/a/36530121
        const dt = 2*Math.PI / 5;
        const radius = PIXELS_PER_TILE / 2;
        context.beginPath();
        for (let i = 0; i < 5; i++) {
            const theta = Math.PI/2 + dt*i;
            context.lineTo(
                this.#x - radius*Math.cos(theta),
                this.#y - radius*Math.sin(theta)
            );
        }
        context.fill();
    }
}
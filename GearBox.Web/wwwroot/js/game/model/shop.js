import { getColorStringFromJson } from "./color.js";
import { PIXELS_PER_TILE } from "./constants.js";
import { Inventory, InventoryDeserializer } from "./item.js";

export class Shop {
    #name;
    #xInPixels;
    #yInPixels;
    #color;
    #options;

    /**
     * 
     * @param {string} name 
     * @param {number} xInPixels 
     * @param {number} yInPixels 
     * @param {string} color 
     * @param {Inventory} options 
     */
    constructor(name, xInPixels, yInPixels, color, options) {
        this.#name = name;
        this.#xInPixels = xInPixels;
        this.#yInPixels = yInPixels;
        this.#color = color;
        this.#options = options;
    }

    get options() { return this.#options; }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        context.fillStyle = this.#color;
        context.fillRect(this.#xInPixels, this.#yInPixels, PIXELS_PER_TILE, PIXELS_PER_TILE);

        context.fillStyle = "black";
        context.fillText(this.#name, this.#xInPixels, this.#yInPixels - PIXELS_PER_TILE / 4);
    }
}

export class ShopInitDeserializer {
    #inventoryDeserializer;

    /**
     * @param {InventoryDeserializer} inventoryDeserializer 
     */
    constructor(inventoryDeserializer) {
        this.#inventoryDeserializer = inventoryDeserializer;
    }

    deserialize(json) {
        const result = new Shop(
            json.name,
            json.xInPixels,
            json.yInPixels,
            getColorStringFromJson(json.color),
            this.#inventoryDeserializer.deserialize(json.options)
        );
        return result;
    }
}
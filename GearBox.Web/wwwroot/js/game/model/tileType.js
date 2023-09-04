import { PIXELS_PER_TILE } from "./constants.js";

export class TileType {
    #color;
    #isTangible;

    /**
     * @param {string} color the CSS color of this tile type
     * @param {boolean} isTangible whether this tile type is tangible
     */
    constructor(color, isTangible) {
        this.#color = color;
        this.#isTangible = isTangible;
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context the canvas context to draw on
     * @param {number} x the x-coodinate, in pixels, of the upper-left corner
     * @param {number} y the y-coodinate, in pixels, of the upper-left corner 
     */
    drawAt(context, x, y) {
        // draw outline
        const grey = (this.#isTangible) ? 100 : 200;
        context.fillStyle = `rgb(${grey} ${grey} ${grey})`;
        context.fillRect(x, y, PIXELS_PER_TILE, PIXELS_PER_TILE);

        // draw inner portion
        const offset = 10;
        context.fillStyle = this.#color;
        context.fillRect(
            x + offset, 
            y + offset,
            PIXELS_PER_TILE - 2*offset,
            PIXELS_PER_TILE - 2*offset
        );
    }
}


/**
 * 
 * @param {{color: {red: number, green: number, blue: number}, isTangible: boolean}} obj 
 * @returns {TileType}
 */
export function deserializeTileTypeJson(obj) {
    const r = obj.color.red;
    const g = obj.color.green;
    const b = obj.color.blue;
    const color = `rgb(${r} ${g} ${b})`; // CSS color doesn't use commas
    // https://developer.mozilla.org/en-US/docs/Web/CSS/color_value/rgb

    return new TileType(color, obj.isTangible);
}
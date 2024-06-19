import { PIXELS_PER_TILE } from "./constants.js";
import { TileType } from "./tileType.js";

// avoids conflicts with build-in JS Map class
export class WorldMap {
    #tileMap;
    #tileTypes;
    
    /**
     * @param {number[][]} tileMap `[y][x]` is a key in `tileTypes`
     * @param {Map<number, TileType>} tileTypes maps values in `tileMap` to a type of tile
     */
    constructor(tileMap, tileTypes) {
        this.#tileMap = tileMap;
        this.#tileTypes = tileTypes;
    }

    /**
     * @returns {number}
     */
    get widthInPixels() {
        return this.#tileMap[0].length * PIXELS_PER_TILE;
    }

    /**
     * @returns {number}
     */
    get heightInPixels() {
        return this.#tileMap.length * PIXELS_PER_TILE;
    }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        // BUG: needs to draw other tile heights after pits
        // need to draw bottom to top so pits render correctly
        for (let yIdx = this.#tileMap.length - 1; yIdx >= 0; yIdx--) {
            const row = this.#tileMap[yIdx];
            row.forEach((tileTypeIdx, xIdx) => {
                const tileType = this.#tileTypes.get(tileTypeIdx);
                tileType.drawAt(context, xIdx * PIXELS_PER_TILE, yIdx * PIXELS_PER_TILE);
            });
        }
    }
}

/**
 * 
 * @param {object} obj 
 * @returns {WorldMap}
 */
export function deserializeMapJson(obj) {
    const tileTypes = new Map();
    obj.tileTypes.forEach(tileType => {
        tileTypes.set(tileType.key, TileType.FromJson(tileType.value));
    });

    return new WorldMap(obj.tileMap, tileTypes);
}
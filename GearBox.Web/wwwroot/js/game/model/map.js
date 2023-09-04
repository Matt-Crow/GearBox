import { PIXELS_PER_TILE } from "./constants.js";
import { TileType, deserializeTileTypeJson } from "./tileType.js";

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
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        this.#tileMap.forEach((row, yIdx) => {
            row.forEach((tileTypeIdx, xIdx) => {
                const tileType = this.#tileTypes.get(tileTypeIdx);
                tileType.drawAt(context, xIdx * PIXELS_PER_TILE, yIdx * PIXELS_PER_TILE);
            })
        });
    }
}

/**
 * 
 * @param {{tileTypes: {key: number, value: {color: {red: number, green: number, blue: number}, isTangible: boolean}}[], tileMap: number[][]}} obj 
 * @returns {WorldMap}
 */
export function deserializeMapJson(obj) {
    const tileTypes = new Map();
    obj.tileTypes.forEach(tileType => {
        tileTypes.set(tileType.key, deserializeTileTypeJson(tileType.value));
    });

    return new WorldMap(obj.tileMap, tileTypes);
}
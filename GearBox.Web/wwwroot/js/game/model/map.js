import { getColorStringFromJson } from "./color.js";
import { PIXELS_PER_TILE } from "./constants.js";

// avoids conflicts with build-in JS Map class
export class TileMap {
    #width;
    #height;
    #pits;
    #floors;
    #walls;
    
    /**
     * @param {number} width in pixels
     * @param {number} height in pixels
     * @param {Tile[]} pits 
     * @param {Tile[]} floors 
     * @param {Tile[]} walls 
     */
    constructor(width, height, pits, floors, walls) {
        this.#width = width;
        this.#height = height;
        this.#pits = pits;
        this.#floors = floors;
        this.#walls = walls;
    }

    /**
     * @returns {number}
     */
    get widthInPixels() { return this.#width; }

    /**
     * @returns {number}
     */
    get heightInPixels() { return this.#height; }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    drawPitsAndFloor(context) {
        this.#pits.forEach(t => t.drawPit(context));
        this.#floors.forEach(t => t.drawFloor(context));
    }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    drawWalls(context) {
        this.#walls.forEach(t => t.drawWall(context));
    }

    /**
     * @param {object} json 
     * @returns {TileMap}
     */
    static fromJson(json) {
        const pits = json.pits.map(j => Tile.fromJson(j));
        const floors = json.floors.map(j => Tile.fromJson(j));
        const walls = json.walls.map(j => Tile.fromJson(j));
        return new TileMap(json.width, json.height, pits, floors, walls);
    }
}

class Tile {
    #color;
    #x;
    #y;

    constructor(color, x, y) {
        this.#color = color;
        this.#x = x;
        this.#y = y;
    }

    /**
     * @param {object} json 
     * @returns {Tile}
     */
    static fromJson(json) {
        const result = new Tile(getColorStringFromJson(json.color), json.x, json.y);
        return result;
    }

    drawPit(context) {
        const offset = PIXELS_PER_TILE / 3;

        context.fillStyle = "black";
        context.fillRect(this.#x, this.#y, PIXELS_PER_TILE, offset);

        context.fillStyle = this.#color;
        context.fillRect(this.#x, this.#y + offset, PIXELS_PER_TILE, PIXELS_PER_TILE);
    }

    drawFloor(context) {
        // draw outline
        context.fillStyle = "rgb(200 200 200)";
        context.fillRect(this.#x, this.#y, PIXELS_PER_TILE, PIXELS_PER_TILE);

        this.#drawInnerTile(context, 0);
    }

    drawWall(context) {
        const offset = PIXELS_PER_TILE / 3;

        // draw outline
        context.fillStyle = "rgb(100, 100, 100)";
        context.fillRect(this.#x, this.#y - offset, PIXELS_PER_TILE, PIXELS_PER_TILE + offset);
        
        this.#drawInnerTile(context, -offset);
    }

    #drawInnerTile(context, yOffset) {
        const offset = 10;
        context.fillStyle = this.#color;
        context.fillRect(
            this.#x + offset, 
            this.#y + offset + yOffset,
            PIXELS_PER_TILE - 2*offset,
            PIXELS_PER_TILE - 2*offset
        );
    }
}
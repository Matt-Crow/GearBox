import { PIXELS_PER_TILE } from "./constants.js";

// avoids conflicts with build-in JS Map class
export class WorldMap {
    #width;
    #height;
    #pits;
    #floors;
    #walls;
    
    /**
     * @param {number} width in pixels
     * @param {number} height in pixels
     * @param {TileSet[]} pits 
     * @param {TileSet[]} floors 
     * @param {TileSet[]} walls 
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
        this.#pits.forEach(tileSet => tileSet.draw(context));
        this.#floors.forEach(tileSet => tileSet.draw(context));
    }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    drawWalls(context) {
        this.#walls.forEach(tileSet => tileSet.draw(context));
    }

    /**
     * @param {object} json 
     * @returns {WorldMap}
     */
    static fromJson(json) {
        const pits = json.pits.map(j => TileSet.fromJson(j));
        const floors = json.floors.map(j => TileSet.fromJson(j));
        const walls = json.walls.map(j => TileSet.fromJson(j));
        return new WorldMap(json.width, json.height, pits, floors, walls);
    }
}

class TileSet {
    #tileType;
    #coordinates;

    /**
     * @param {TileType} tileType 
     */
    constructor(tileType, coordinates) {
        this.#tileType = tileType;
        this.#coordinates = coordinates;
    }

    /**
     * @param {object} json 
     * @returns {TileSet}
     */
    static fromJson(json) {
        const result = new TileSet(TileType.fromJson(json.tileType), json.coordinates.map(j => [j.x, j.y]));
        return result;
    }

    /**
     * @param {CanvasRenderingContext2D} context
     */
    draw(context) {
        this.#coordinates.forEach(c => this.#tileType.drawAt(context, c[0], c[1]));
    }
}

class TileType {
    #color;
    #draw;

    /**
     * @param {string} color the CSS color of this tile type
     * @param {(CanvasRenderingContext2D, number, number, string) => none} draw 
     */
    constructor(color, draw) {
        this.#color = color;
        this.#draw = draw;
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context the canvas context to draw on
     * @param {number} x the x-coodinate, in pixels, of the upper-left corner
     * @param {number} y the y-coodinate, in pixels, of the upper-left corner 
     */
    drawAt(context, x, y) {
        this.#draw(context, x, y, this.#color);
    }

    static fromJson(json) {
        const r = json.color.red;
        const g = json.color.green;
        const b = json.color.blue;
        const color = `rgb(${r} ${g} ${b})`; // CSS color doesn't use commas
        // https://developer.mozilla.org/en-US/docs/Web/CSS/color_value/rgb

        let draw = () => {throw new Error("Not implemented")};
        if (json.height > 0) {
            draw = drawWall;
        } else if (json.height < 0) {
            draw = drawPit;
        } else { // json.height === 0
            draw = drawFloor;
        }

        return new TileType(color, draw);
    }
}

function drawWall(context, x, y, color) {
    const offset = PIXELS_PER_TILE / 3;

    // draw outline
    context.fillStyle = "rgb(100, 100, 100)";
    context.fillRect(x, y - offset, PIXELS_PER_TILE, PIXELS_PER_TILE + offset);
    
    drawInnerTile(context, x, y - offset, color);
}

function drawFloor(context, x, y, color) {
    // draw outline
    context.fillStyle = "rgb(200 200 200)";
    context.fillRect(x, y, PIXELS_PER_TILE, PIXELS_PER_TILE);

    drawInnerTile(context, x, y, color);
}

function drawPit(context, x, y, color) {
    context.fillStyle = "black";
    context.fillRect(x, y, PIXELS_PER_TILE, PIXELS_PER_TILE / 3);

    context.fillStyle = color;
    context.fillRect(x, y + PIXELS_PER_TILE / 3, PIXELS_PER_TILE, PIXELS_PER_TILE);
}

function drawInnerTile(context, x, y, color) {
    const offset = 10;
    context.fillStyle = color;
    context.fillRect(
        x + offset, 
        y + offset,
        PIXELS_PER_TILE - 2*offset,
        PIXELS_PER_TILE - 2*offset
    );
}
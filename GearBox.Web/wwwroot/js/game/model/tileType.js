import { PIXELS_PER_TILE } from "./constants.js";

export class TileType {
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

    static FromJson(json) {
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
    // draw outline
    context.fillStyle = "rgb(100, 100, 100)";
    context.fillRect(x, y, PIXELS_PER_TILE, PIXELS_PER_TILE);
    
    drawInnerTile(context, x, y, color);
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
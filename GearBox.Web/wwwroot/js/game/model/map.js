export class Map {
    #tileMap;
    #tileTypes;
    
    constructor({tileMap, tileTypes}) {
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

                // draw border
                if (tileType.isTangible) {
                    context.fillStyle = "rgb(100 100 100)";
                } else {
                    context.fillStyle = "rgb(200 200 200)";
                }
                context.fillRect(xIdx * 100, yIdx * 100, 100, 100);

                context.fillStyle = tileType.color;
                context.fillRect(xIdx * 100 + 10, yIdx * 100 + 10, 80, 80);
            })
        });
    }
}
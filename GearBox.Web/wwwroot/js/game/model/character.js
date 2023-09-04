import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";

export class Character {
    #xInPixels;
    #yInPixels;

    constructor({xInPixels, yInPixels}) {
        this.#xInPixels = xInPixels;
        this.#yInPixels = yInPixels;
    }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        context.fillStyle = "rgb(0, 255, 0)";
        context.beginPath();
        context.arc(this.#xInPixels, this.#yInPixels, 50, 0, 2*Math.PI);
        context.fill();
    }
}

export class CharacterJsonDeserializer extends JsonDeserializer {
    constructor() {
        super("character", (obj) => new Character(obj));
    }
}
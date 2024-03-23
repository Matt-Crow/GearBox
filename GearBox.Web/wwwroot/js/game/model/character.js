import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { PIXELS_PER_TILE } from "./constants.js";

export class Character {
    #id;
    #color = "rgb(0 255 0)"; // todo read from server
    #x;
    #y;
    #hitPoints;

    /**
     * @param {string} id a unique identifier for this character (GUID)
     * @param {number} x the x-coordinate of this character's center, in pixels
     * @param {number} y the y-coordinate of this character's center, in pixels 
     * @param {Fraction} hitPoints 
     */
    constructor(id, x, y, hitPoints) {
        this.#id = id;
        this.#x = x;
        this.#y = y;
        this.#hitPoints = hitPoints;
    }

    /**
     * @returns {string} a unique identifier for this character (GUID)
     */
    get id() { return this.#id; }

    /**
     * @returns {number} the x-coordinate of this character's center, in pixels
     */
    get x() { return this.#x; }

    /**
     * @returns {number} the y-coordinate of this character's center, in pixels
     */
    get y() { return this.#y; }

    get hitPoints() { return this.#hitPoints; }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        const r = PIXELS_PER_TILE / 2;
        context.fillStyle = this.#color;
        
        // https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/arc
        context.beginPath();
        context.arc(this.#x, this.#y, r, 0, 2*Math.PI);
        context.fill();
    }
}

export class Fraction {
    #current;
    #max;

    constructor(current, max) {
        this.#current = current;
        this.#max = max;
    }

    get current() {
        return this.#current;
    }

    get max() {
        return this.#max;
    }
}

export class CharacterJsonDeserializer extends JsonDeserializer {
    constructor() {
        super("character", (obj) => new Character(obj.id, obj.x, obj.y, new Fraction(obj.hitPoints.current, obj.hitPoints.max)));
    }
}
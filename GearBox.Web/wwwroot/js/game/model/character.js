import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { getColorStringFromJson } from "./color.js";
import { PIXELS_PER_TILE } from "./constants.js";

export class Character {
    #id;
    #name;
    #level;
    #color;
    #x;
    #y;
    #hitPoints;

    /**
     * @param {string} id a unique identifier for this character (GUID)
     * @param {string} name 
     * @param {number} level 
     * @param {string} color 
     * @param {number} x the x-coordinate of this character's center, in pixels
     * @param {number} y the y-coordinate of this character's center, in pixels 
     * @param {Fraction} hitPoints 
     */
    constructor(id, name, level, color, x, y, hitPoints) {
        this.#id = id;
        this.#name = name;
        this.#level = level;
        this.#color = color;
        this.#x = x;
        this.#y = y;
        this.#hitPoints = hitPoints;
    }

    /**
     * @returns {string} a unique identifier for this character (GUID)
     */
    get id() { return this.#id; }

    get name() { return this.#name; }

    get level() { return this.#level; }

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

        context.fillStyle = "black";
        context.fillText(`${this.#name} LV ${this.#level} (${this.#hitPoints.current} HP)`, this.#x-r, this.#y-r);
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

export const characterDeserializer = new JsonDeserializer("character", (json) => new Character(
    json.id, 
    json.name,
    json.level,
    getColorStringFromJson(json.color),
    json.x, 
    json.y, 
    new Fraction(json.hitPoints.current, json.hitPoints.max)
));
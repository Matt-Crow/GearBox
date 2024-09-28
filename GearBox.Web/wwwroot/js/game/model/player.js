import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { Character, Fraction } from "./character.js";
import { getColorStringFromJson } from "./color.js";

export class Player extends Character {
    #energy;

    /**
     * @param {string} id 
     * @param {string} name 
     * @param {number} level 
     * @param {string} color 
     * @param {number} x the x-coordinate of this character's center, in pixels
     * @param {number} y the y-coordinate of this character's center, in pixels 
     * @param {Fraction} hitPoints
     * @param {Fraction} energy 
     */
    constructor(id, name, level, color, x, y, hitPoints, energy) {
        super(id, name, level, color, x, y, hitPoints);
        this.#energy = energy;
    }

    get energy() { return this.#energy; }

    static fromJson(json) {
        var result = new Player(
            json.id, 
            json.name,
            json.level,
            getColorStringFromJson(json.color),
            json.x,
            json.y,
            new Fraction(json.hitPoints.current, json.hitPoints.max),
            new Fraction(json.energy.current, json.energy.max)
        );
        return result;
    }
}

export class PlayerJsonDeserializer extends JsonDeserializer {
    constructor() {
        super("playerCharacter", json => Player.fromJson(json));
    }
}

export class PlayerStatSummary {
    #lines;

    /**
     * @param {string[]} lines 
     */
    constructor(lines) {
        this.#lines = lines;
    }

    get lines() { return this.#lines; }

    /**
     * @param {object} json 
     * @returns PlayerStatSummary
     */
    static fromJson(json) {
        return new PlayerStatSummary(json.lines);
    }
}
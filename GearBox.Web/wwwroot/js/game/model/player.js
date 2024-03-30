/*
    Exports:
    - Player: model class
    - Fraction: used by Player
    - PlayerChangeHandler: deserializes and responds to player updates
 */

import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { Character, Fraction } from "./character.js";

export class Player extends Character {
    #energy;

    /**
     * @param {string} id 
     * @param {number} x the x-coordinate of this character's center, in pixels
     * @param {number} y the y-coordinate of this character's center, in pixels 
     * @param {Fraction} hitPoints
     * @param {Fraction} energy 
     */
    constructor(id, x, y, hitPoints, energy) {
        super(id, x, y, hitPoints);
        this.#energy = energy;
    }

    get energy() { return this.#energy; }
}

export class PlayerChangeHandler extends JsonDeserializer {
    #playerId;
    #changeListener;

    constructor(playerId, changeListener) {
        super("playerCharacter", obj => this.#handle(obj));
        this.#playerId = playerId;
        this.#changeListener = changeListener;
    }

    /**
     * @param {object} obj 
     * @returns {Player}
     */
    #handle(obj) {
        const player = this.#deserialize(obj);
        if (player.id === this.#playerId) {
            this.#changeListener(player);
        }
        return player;
    }

    #deserialize(json) {
        var result = new Player(
            json.id, 
            json.x,
            json.y,
            new Fraction(json.hitPoints.current, json.hitPoints.max),
            new Fraction(json.energy.current, json.energy.max)
        );
        return result;
    }
}
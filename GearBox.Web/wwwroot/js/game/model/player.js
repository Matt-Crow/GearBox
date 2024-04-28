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
     * @param {string} name 
     * @param {number} level 
     * @param {number} x the x-coordinate of this character's center, in pixels
     * @param {number} y the y-coordinate of this character's center, in pixels 
     * @param {Fraction} hitPoints
     * @param {Fraction} energy 
     */
    constructor(id, name, level, x, y, hitPoints, energy) {
        super(id, name, level, x, y, hitPoints);
        this.#energy = energy;
    }

    get energy() { return this.#energy; }
}

export class PlayerChangeHandler extends JsonDeserializer {
    #playerId;
    #changeListener;
    #weaponChangeHandler;

    constructor(playerId, changeListener, weaponChangeHandler) {
        super("playerCharacter", obj => this.#handle(obj));
        this.#playerId = playerId;
        this.#changeListener = changeListener;
        this.#weaponChangeHandler = weaponChangeHandler;
    }

    /**
     * @param {object} json 
     * @returns {Player}
     */
    #handle(json) {
        const player = this.#deserialize(json);
        if (player.id === this.#playerId) {
            this.#changeListener(player);
            this.#handleWeaponChange(json.weapon);
        }
        return player;
    }

    #deserialize(json) {
        var result = new Player(
            json.id, 
            json.name,
            json.level,
            json.x,
            json.y,
            new Fraction(json.hitPoints.current, json.hitPoints.max),
            new Fraction(json.energy.current, json.energy.max)
        );
        return result;
    }

    #handleWeaponChange(json) {
        if (json.hasChanged) {
            const weaponSlot = JSON.parse(json.body);
            this.#weaponChangeHandler.handleContent(weaponSlot);
        }
    }
}
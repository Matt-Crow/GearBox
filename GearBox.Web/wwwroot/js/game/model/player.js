/*
    Exports:
    - Player: model class
    - Fraction: used by Player
    - PlayerChangeHandler: deserializes and responds to player updates
 */

import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";

export class Player {
    #id;
    #energy;

    /**
     * @param {string} id 
     * @param {Fraction} energy 
     */
    constructor(id, energy) {
        this.#id = id;
        this.#energy = energy;
    }

    get id() { return this.#id; }
    get energy() { return this.#energy; }
    get dynamicValues() { return this.#energy.dynamicValues }
}

export class Fraction {
    #current;
    #max;

    constructor(current, max) {
        this.#current = current;
        this.#max = max;
    }

    get current() { return this.#current; }
    get max() { return this.#max; }
    get dynamicValues() { return [this.#current, this.#max]; }
}

export class PlayerChangeHandler extends JsonDeserializer {
    #playerId;
    #previous = null;
    #changeListener;

    constructor(playerId, changeListener) {
        super("playerCharacter", obj => this.#handle(obj));
        this.#playerId = playerId;
        this.#changeListener = changeListener;
    }

    #handle(obj) {
        const player = this.#deserialize(obj);
        if (player.id === this.#playerId && this.#hasPlayerChanged(player)) {
            // stops flickering in the UI
            this.#previous = player;
            this.#changeListener(player);
        }
        return {draw: () => null}; // hacky for now: don't want player to be returned by World::player
        // uncomment this next line once Player extends Character
        // return player;
    }

    #deserialize(json) {
        var result = new Player(
            json.id, 
            new Fraction(json.energy.current, json.energy.max)
        );
        return result;
    }

    /**
     * @param {Player} player 
     */
    #hasPlayerChanged(player) {
        const result = this.#previous === null || this.#arraysDiffer(player.dynamicValues, this.#previous.dynamicValues);
        return result;
    }

    /**
     * @param {any[]} array1 
     * @param {any[]} array2 
     * @returns 
     */
    #arraysDiffer(array1, array2) {
        if (array1.length !== array2.length) {
            return true;
        }
        for (let i = 0; i < array1.length; i++) {
            if (array1[i] !== array2[i]) {
                return true;
            }
        }
        return false;
    }
}
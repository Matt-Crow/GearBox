import { Character } from "../model/character.js";
import { Player } from "../model/player.js";

export class PlayerHud {
    #element;
    #playerUpdateListener;
    #characterSupplier;

    /**
     * @param {HTMLDivElement} element 
     */
    constructor(element) {
        this.#element = element;
        this.#playerUpdateListener = p => this.#bind(p);
        this.#characterSupplier = () => null;
    }

    /**
     * @returns {(p: Player) => any}
     */
    get playerUpdateListener() { return this.#playerUpdateListener; }

    /**
     * @param {() => Character} value 
     */
    set characterSupplier(value) { this.#characterSupplier = value; }

    /**
     * @param {Player} player 
     */
    #bind(player) {
        const character = this.#characterSupplier();
        bind(this.#element, "currentHP", character?.hitPoints.current);
        bind(this.#element, "maxHP", character?.hitPoints.max);
        bind(this.#element, "currentEnergy", player.energy.current);
        bind(this.#element, "maxEnergy", player.energy.max);
    }
}

/**
 * Sets the innerText of child elements under the given parent
 * @param {HTMLElement} element the parent element
 * @param {string} className the class name of the children
 * @param {string} innerText the innerText to set on the children
 */
function bind(element, className, innerText) {
    const children = Array.from(element.getElementsByClassName(className));
    if (!children) {
        throw new Error(`Failed to locate any elements with className "${className}" under ${element}`);
    }
    children.forEach(child => child.innerText = innerText);
}
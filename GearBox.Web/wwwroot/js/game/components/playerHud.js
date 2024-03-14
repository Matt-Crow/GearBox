import { Player, PlayerEventListener } from "../model/player.js";

export class PlayerHud {
    #element;
    #playerEventListener;

    /**
     * @param {HTMLDivElement} element 
     */
    constructor(element) {
        this.#element = element;
        this.#playerEventListener = new PlayerEventListener({
            onPlayerChanged: p => this.#bind(p)
        });        
    }

    /**
     * @returns {PlayerEventListener}
     */
    get playerEventListener() {
        return this.#playerEventListener;
    }

    /**
     * @param {Player} player 
     */
    #bind(player) {
        bind(this.#element, "currentHP", player.hitPoints.current);
        bind(this.#element, "maxHP", player.hitPoints.max);
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
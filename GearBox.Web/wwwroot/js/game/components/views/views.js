import { Switcher } from "../shared/switcher.js";

/*
    Manages the main views div
*/
export class Views {
    #selector;
    #switcher;
    #wasAliveLastTime = null;

    /**
     * @param {string} selector 
     */
    constructor(selector) {
        this.#selector = selector;
        this.#switcher = new Switcher(this.#selector);
        this.#show(".view-loading");
    }

    #show(viewSelector) {
        this.#switcher.show(`${this.#selector} ${viewSelector}`);
    }

    /**
     * @param {Area} area 
    */
    handleAreaUpdate(area) {
        const aliveNow = !!area.player;
        if (aliveNow === this.#wasAliveLastTime) {
           return; // no changes, so don't flicker the screen
        }

        this.#wasAliveLastTime = aliveNow;
        if (aliveNow) {
            this.#show(".view-alive");
        } else {
            this.#show(".view-dead");
        }
    }
}
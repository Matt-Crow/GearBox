import { Switcher } from "../shared/switcher.js";
import { ViewAlive } from "./alive/viewAlive.js";

/*
    Manages the main views div
*/
export class Views {
    #selector;
    #switcher;
    #viewAlive;
    #wasAliveLastTime = null;

    constructor() {
        this.#selector = "#views";
        this.#switcher = new Switcher(this.#selector);
        this.#viewAlive = new ViewAlive();
        this.#show(".view-loading");
    }

    /**
     * @returns {ViewAlive}
     */
    get viewAlive() { return this.#viewAlive; }

    #show(viewSelector) {
        this.#switcher.show(`${this.#selector} ${viewSelector}`);
    }

    /**
     * @param {Area} area 
    */
    handleAreaUpdate(area) {
        this.#viewAlive.handleAreaUpdate(area);

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
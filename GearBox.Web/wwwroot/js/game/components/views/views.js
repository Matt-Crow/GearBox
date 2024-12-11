import { Client } from "../../infrastructure/client.js";
import { UiStateChanges } from "../../model/areaUpdate.js";
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

    /**
     * @param {Client} client 
     */
    constructor(client) {
        this.#selector = "#views";
        this.#switcher = new Switcher(this.#selector);
        this.#viewAlive = new ViewAlive(client);
        this.#show(".view-loading");
    }

    spawnHtml() {
        this.#viewAlive.spawnHtml();
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

    /**
     * @param {UiStateChanges} uiStateChanges 
     */
    handleUiStateChanges(uiStateChanges) {
        this.#viewAlive.handleUiStateChanges(uiStateChanges);
    }
}
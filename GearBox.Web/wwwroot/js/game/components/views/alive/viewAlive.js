import { Area } from "../../../model/area.js";
import { PlayerHud } from "./playerHud.js";
import { Canvas } from "./canvas.js";

/*
    Shows when the player is alive
*/
export class ViewAlive {
    #selector = "#views .view-alive";
    #canvas;
    #playerHud;

    constructor() {
        const element = document.querySelector(this.#selector);
        this.#canvas = new Canvas(element.querySelector(".canvas"));
        this.#playerHud = new PlayerHud(element.querySelector("#player-hud"));
    }

    spawnHtml() {
        this.#playerHud.spawnHtml();
    }

    /**
     * @returns {Canvas}
     */
    get canvas() { return this.#canvas; }

    /**
     * @returns {PlayerHud}
     */
    get playerHud() { return this.#playerHud; }

    /**
     * @param {Area} area 
    */
    handleAreaUpdate(area) {
        if (area.player) {
            this.#playerHud.bindIfChanged(area.player);
        }
    }
}
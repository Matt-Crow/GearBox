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

    /**
     * @returns {Canvas}
     */
    get canvas() { return this.#canvas; }

    /**
     * @param {Area} area 
    */
    handleAreaUpdate(area) {
        if (area.player) {
            this.#playerHud.bindIfChanged(area.player);
        }
    }
}
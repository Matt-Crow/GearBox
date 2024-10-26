import { Area } from "../../../model/area.js";
import { PlayerHud } from "./playerHud.js";
import { Canvas } from "./canvas.js";
import { UiStateChanges } from "../../../model/areaUpdate.js";
import { Client } from "../../../infrastructure/client.js";
import { MainModal } from "../../mainModal.js";

/*
    Shows when the player is alive
*/
export class ViewAlive {
    #selector = "#views .view-alive";
    #canvas;
    #playerHud;
    #mainModal;

    /**
     * @param {Client} client 
     */
    constructor(client) {
        const element = document.querySelector(this.#selector);
        this.#canvas = new Canvas(element.querySelector(".canvas"));
        this.#playerHud = new PlayerHud(element.querySelector("#player-hud"));
        this.#mainModal = new MainModal("#main-modal", client);
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
     * @returns {MainModal}
     */
    get mainModal() { return this.#mainModal; }

    /**
     * @param {Area} area 
    */
    handleAreaUpdate(area) {
        if (area.player) {
            this.#playerHud.bindIfChanged(area.player);
        }
    }

    /**
     * @param {UiStateChanges} uiStateChanges 
     */
    handleUiStateChanges(uiStateChanges) {
        this.#playerHud.handleUiStateChanges(uiStateChanges);
        this.#mainModal.handleUiStateChanges(uiStateChanges);
    }
}
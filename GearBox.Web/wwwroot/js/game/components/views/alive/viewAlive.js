import { Canvas } from "./canvas.js";

/*
    Shows when the player is alive
*/
export class ViewAlive {
    #selector = "#views .view-alive";
    #canvas;

    constructor() {
        const element = document.querySelector(this.#selector);
        this.#canvas = new Canvas(element.querySelector(".canvas"));
    }

    /**
     * @returns {Canvas}
     */
    get canvas() { return this.#canvas; }
}
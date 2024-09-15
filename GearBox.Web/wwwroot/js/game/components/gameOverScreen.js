import { Client } from "../infrastructure/client.js";
import { Area } from "../model/area.js";

export class GameOverScreen {
    #showWhenAlive;
    #showWhenDead;
    #wasAliveLastTime = null;

    /**
     * @param {HTMLElement} showWhenAlive 
     * @param {HTMLElement} showWhenDead 
     * @param {HTMLButtonElement} respawnButton 
     * @param {Client} client 
     */
    constructor(showWhenAlive, showWhenDead, respawnButton, client) {
        this.#showWhenAlive = showWhenAlive;
        this.#showWhenDead = showWhenDead;
        $(respawnButton).on('click', () => client.respawn());
        $(this.#showWhenAlive).hide();
        $(this.#showWhenDead).hide();
    }

    /**
     * 
     * @param {Area} area 
     */
    update(area) {
        const aliveNow = !!area.player;
        if (aliveNow === this.#wasAliveLastTime) {
            return; // no changes, so don't flicker the screen
        }
        this.#wasAliveLastTime = aliveNow;
        if (aliveNow) {
            $(this.#showWhenAlive).show();
            $(this.#showWhenDead).hide();
        } else {
            $(this.#showWhenAlive).hide();
            $(this.#showWhenDead).show();
        }
    }
}
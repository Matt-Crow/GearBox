import { Client } from "../infrastructure/client.js";
import { World } from "../model/world.js";

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
     * @param {World} world 
     */
    update(world) {
        const aliveNow = !!world.player;
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
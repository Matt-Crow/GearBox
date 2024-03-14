/**
 * This module is responsible for a player's Heads Up Display.
 */

import { Player, PlayerEventListener } from "../model/player.js";

export class PlayerHud extends HTMLElement {
    #playerEventListener;

    constructor() {
        super();
        this.#playerEventListener = new PlayerEventListener({
            onPlayerChanged: p => this.#bind(p)
        });
    }

    get playerEventListener() {
        return this.#playerEventListener;
    }

    connectedCallback() {
        // do not use shadow DOM, as that messes with Bootstrap!
        // https://stackoverflow.com/a/67210319

        const content = document.createElement("div");
        content.classList.add("d-flex");
        content.classList.add("justify-content-between");
        this.appendChild(content);

        const hitPointWidget = document.createElement("div");
        hitPointWidget.appendChild(document.createTextNode("HP: "));
        hitPointWidget.appendChild(createBindableSpan("currentHP"));
        hitPointWidget.appendChild(document.createTextNode("/"));
        hitPointWidget.appendChild(createBindableSpan("maxHP"));
        content.appendChild(hitPointWidget);

        const energyWidget = document.createElement("div");
        energyWidget.appendChild(document.createTextNode("Energy: "));
        energyWidget.appendChild(createBindableSpan("currentEnergy"));
        energyWidget.appendChild(document.createTextNode("/"));
        energyWidget.appendChild(createBindableSpan("maxEnergy"));
        content.appendChild(energyWidget);
    }

    /**
     * @param {Player} player 
     */
    #bind(player) {
        $("#currentHP").text(player.hitPoints.current);
        $("#maxHP").text(player.hitPoints.max);
        $("#currentEnergy").text(player.energy.current);
        $("#maxEnergy").text(player.energy.max);
    }
}

function createBindableSpan(id) {
    const element = document.createElement("span");
    element.id = id;
    return element;
}

window.customElements.define("player-hud", PlayerHud);
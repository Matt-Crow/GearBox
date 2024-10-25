import { ActiveAbility } from "../../../model/activeAbility.js";
import { Player } from "../../../model/player.js";
import { ActiveAbilityHud } from "./activeAbilityHud.js";

export class PlayerHud {
    #element;
    #activeHuds = [];
    #previous = null;

    /**
     * @param {HTMLDivElement} element 
     */
    constructor(element) {
        this.#element = element;
    }

    spawnHtml() {
        this.#activeHuds = [];
        const $actives = $("#actives");
        $actives.empty();
        for (let i = 1; i <= 9; i++) {
            $actives.append($("<div>").addClass(`active-${i} flex-fill`));
            
            const activeHud = new ActiveAbilityHud(`#actives .active-${i}`, i);
            activeHud.spawnHtml();
            this.#activeHuds.push(activeHud);
        }
    }

    /**
     * @param {Player} player 
     */
    bindIfChanged(player) {
        // stops flickering in the UI
        if (this.#previous === null || this.#hasPlayerChanged(player)) {
            this.#bind(player);
            this.#previous = player;
        }
    }

    /**
     * @param {ActiveAbility[]} actives 
     */
    setActives(actives) {
        this.#activeHuds.forEach((hud, i) => {
            const active = actives.length <= i ? null : actives[i];
            hud.setActive(active);
        });
    }

    /**
     * @param {Player} player 
    */
    #hasPlayerChanged(player) {
        const props = [
            p => p.hitPoints.current,
            p => p.hitPoints.max,
            p => p.energy.current,
            p => p.energy.max
         ];
         const result = this.#previous === null || props.some(prop => prop(player) !== prop(this.#previous));
         return result;
    }

    /**
     * @param {Player} player 
     */
    #bind(player) {
        bind(this.#element, "currentHP", player.hitPoints.current);
        bind(this.#element, "maxHP", player.hitPoints.max);
        bind(this.#element, "currentEnergy", player.energy.current);
        bind(this.#element, "maxEnergy", player.energy.max);
    }
}

/**
 * Sets the innerText of child elements under the given parent
 * @param {HTMLElement} element the parent element
 * @param {string} className the class name of the children
 * @param {string} innerText the innerText to set on the children
 */
function bind(element, className, innerText) {
    const children = Array.from(element.getElementsByClassName(className));
    if (!children) {
        throw new Error(`Failed to locate any elements with className "${className}" under ${element}`);
    }
    children.forEach(child => child.innerText = innerText);
}
/*
    This module contains code related to AreaUpdateJson.cs
*/

import { ActiveAbility } from "./activeAbility.js";
import { Inventory, Item } from "./item.js";
import { PassiveAbility } from "./passiveAbility.js";
import { PlayerStatSummary } from "./player.js";
import { OpenShop } from "./shop.js";

export class AreaUpdate {
    #gameObjects;
    #uiStateChanges;

    /**
     * @param {object[]} gameObjects 
     * @param {UiStateChanges} uiStateChanges 
     */
    constructor(gameObjects, uiStateChanges) {
        this.#gameObjects = gameObjects;
        this.#uiStateChanges = uiStateChanges;
    }

    get gameObjects() { return this.#gameObjects; }
    get uiStateChanges() { return this.#uiStateChanges; }
}

export class UiStateChanges {
    #area;
    #inventory;
    #manipulator;
    #torso;
    #summary;
    #actives;
    #passives;
    #openShop;

    /**
     * 
     * @param {*} area 
     * @param {MaybeChange<Inventory>} inventory 
     * @param {MaybeChange<Item?>} manipulator 
     * @param {MaybeChange<Item?>} torso 
     * @param {MaybeChange<PlayerStatSummary>} summary 
     * @param {MaybeChange<ActiveAbility[]} actives 
     * @param {MaybeChange<PassiveAbility[]} passives
     * @param {MaybeChange<OpenShop?>} openShop 
     */
    constructor(
        area,
        inventory,
        manipulator,
        torso,
        summary,
        actives,
        passives,
        openShop
    ) {
        this.#area = area;
        this.#inventory = inventory;
        this.#manipulator = manipulator;
        this.#torso = torso;
        this.#summary = summary;
        this.#actives = actives;
        this.#passives = passives;
        this.#openShop = openShop;
    }

    get inventory() { return this.#inventory; }
    get manipulator() { return this.#manipulator; }
    get torso() { return this.#torso; }
    get summary() { return this.#summary; }
    get actives() { return this.#actives; }
    get passives() { return this.#passives; }
    get openShop() { return this.#openShop; }
}

export class MaybeChange {
    #hasChanged;
    #value;

    /**
     * @param {boolean} hasChanged 
     * @param {any} value 
     */
    constructor(hasChanged, value) {
        this.#hasChanged = hasChanged;
        this.#value = value;
    }

    /**
     * @param {(value: any) => any} handler 
     */
    handleChange(handler) {
        if (this.#hasChanged) {
            handler(this.#value);
        }
    }

    /**
     * @param {object} json 
     * @param {(value: object) => any} deserialize used to convert json.value if it has changed
     * @returns {MaybeChange}
     */
    static fromJson(json, deserialize) {
        const value = json.hasChanged ? deserialize(json.value) : null;
        const result = new MaybeChange(json.hasChanged, value);
        return result;
    }
}
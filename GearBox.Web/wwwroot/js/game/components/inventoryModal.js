import { Client } from "../infrastructure/client.js";
import { Inventory, Item } from "../model/item.js";
import { Player, PlayerEventListener } from "../model/player.js";

export class InventoryModal {
    #modal;
    #materialRows;
    #equipmentRows;
    #playerEventListener;
    #client;

    /**
     * @param {HTMLDialogElement} modal
     * @param {Client} client 
     */
    constructor(modal, client) {
        this.#modal = modal;
        this.#materialRows = document.querySelector("#materialRows");
        this.#equipmentRows = document.querySelector("#equipmentRows");
        this.#playerEventListener = new PlayerEventListener({
            onPlayerChanged: (player) => this.#bind(player),
            onPlayerRemoved: () => this.#clear()
        });
        this.#client = client;
        this.#setWeapon(null);
    }

    get playerEventListener() {
        return this.#playerEventListener;
    }

    toggle() {
        if (this.#modal.open) {
            this.#modal.close();
        } else {
            this.#modal.showModal();
        }
    }

    #clear() {
        this.#materialRows.replaceChildren();
        this.#equipmentRows.replaceChildren();
    }

    /**
     * @param {Player} player 
     */
    #bind(player) {
        this.#setInventory(player.inventory);
        this.#setWeapon(player.weapon);
    }

    /**
     * @param {Inventory} inventory 
     */
    #setInventory(inventory) {
        this.#clear();
        inventory.materials.forEach(item => this.#addMaterial(item));
        inventory.equipment.forEach(item => this.#addEquipment(item));
    }

    /**
     * @param {Item} item 
     */
    #addMaterial(item) {
        const tds = [
            item.type.name,
            item.description,
            item.quantity
        ].map(data => {
            const e = document.createElement("td");
            e.innerText = data;
            return e;
        });
        const tr = document.createElement("tr");
        tds.forEach(td => tr.appendChild(td));
        this.#materialRows.appendChild(tr);
    }

    /**
     * @param {Item} item 
     */
    #addEquipment(item) {
        const tds = [
            item.type.name,
            item.description
        ].map(data => {
            const e = document.createElement("td");
            e.innerText = data;
            return e;
        });
        const tr = document.createElement("tr");
        tds.forEach(td => tr.appendChild(td));

        const equipButton = document.createElement("button");
        equipButton.innerText = "Equip";
        equipButton.type = "button";
        equipButton.classList.add("btn");
        equipButton.classList.add("btn-primary");

        equipButton.addEventListener("click", (e) => this.#client.equip(item.id));
        
        tr.appendChild(equipButton);

        // todo compare on hover

        this.#equipmentRows.appendChild(tr);
    }

    #setWeapon(weapon) {
        if (!weapon) {
            $("#noWeapon").show();
            $("#yesWeapon").hide();
            return;
        }

        $("#noWeapon").hide();
        $("#yesWeapon").show();

        $("#weaponName").text(weapon.type.name);
        $("#weaponDescription").text(weapon.description);
        const details = weapon.details.map(str => {
            const e = document.createElement("li");
            e.innerText = str;
            return e;
        })
        document
            .querySelector("#weaponDetails")
            .replaceChildren(...details); // need to destructure array for some reason
    }
}

/**
 * 
 * @param {HTMLElement} element 
 * @param {string} className
 * @returns {HTMLElement} 
 */
function getDescendantByClassName(element, className) {
    const descendants = Array.from(element.getElementsByClassName(className));
    if (!descendants) {
        throw new Error(`Failed to locate any elements with className ${className} under ${element}`);
    }
    return descendants[0];
}
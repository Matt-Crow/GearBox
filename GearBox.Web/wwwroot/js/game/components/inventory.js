import { Client } from "../infrastructure/client.js";
import { Inventory, Item } from "../model/item.js";
import { PlayerEventListener } from "../model/player.js";

export class InventoryModal {
    #modal;
    #materialRows;
    #equipmentRows;
    #playerEventListener;
    #client;

    /**
     * @param {HTMLDialogElement} modal
     * @param {HTMLTableSectionElement} materialRows
     * @param {HTMLTableSectionElement} equipmentRows
     * @param {Client} client 
     */
    constructor(modal, materialRows, equipmentRows, client) {
        this.#modal = modal;
        this.#materialRows = materialRows;
        this.#equipmentRows = equipmentRows;
        this.#playerEventListener = new PlayerEventListener({
            onPlayerChanged: (player) => this.#setInventory(player.inventory),
            onPlayerRemoved: () => this.#clear()
        });
        this.#client = client;
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
}
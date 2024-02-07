import { Inventory, Item } from "../model/item.js";
import { PlayerEventListener } from "../model/player.js";

export class InventoryModal {
    #modal;
    #tbody;
    #playerEventListener;

    /**
     * @param {HTMLDialogElement} modal
     * @param {HTMLTableSectionElement} tbody 
     */
    constructor(modal, tbody) {
        this.#modal = modal;
        this.#tbody = tbody;
        this.#playerEventListener = new PlayerEventListener({
            onPlayerAdded: (player) => this.#setInventory(player.inventory),
            onPlayerUpdated: (player) => this.#setInventory(player.inventory),
            onPlayerRemoved: () => this.#clear()
        });
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
        this.#tbody.replaceChildren();
    }

    /**
     * @param {Inventory} inventory 
     */
    #setInventory(inventory) {
        this.#clear();
        inventory.equipment.forEach(item => this.#addItem(item));
        inventory.materials.forEach(item => this.#addItem(item));
    }

    /**
     * @param {Item} item 
     */
    #addItem(item) {
        const tds = [
            item.type.name,
            item.type.description ?? "items don't have descriptions yet",
            item.quantity
        ].map(data => {
            const e = document.createElement("td");
            e.innerText = data;
            return e;
        });
        const tr = document.createElement("tr");
        tds.forEach(td => tr.appendChild(td));
        this.#tbody.appendChild(tr);
    }
}
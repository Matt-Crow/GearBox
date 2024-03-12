import { Inventory, Item } from "../model/item.js";
import { PlayerEventListener } from "../model/player.js";

export class InventoryModal {
    #modal;
    #materialRows;
    #equipmentRows;
    #playerEventListener;

    /**
     * @param {HTMLDialogElement} modal
     * @param {HTMLTableSectionElement} materialRows
     * @param {HTMLTableSectionElement} equipmentRows  
     */
    constructor(modal, materialRows, equipmentRows) {
        this.#modal = modal;
        this.#materialRows = materialRows;
        this.#equipmentRows = equipmentRows;
        this.#playerEventListener = new PlayerEventListener({
            onPlayerChanged: (player) => this.#setInventory(player.inventory),
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

        // todo button should say "unequip" if the equipment is equipped
        // todo equip on click - pass the ID to server
        equipButton.addEventListener("click", (e) => console.log({item, e}));
        
        tr.appendChild(equipButton);

        // todo compare on hover

        this.#equipmentRows.appendChild(tr);
    }
}
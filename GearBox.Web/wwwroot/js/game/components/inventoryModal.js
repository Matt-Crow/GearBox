import { ChangeListener } from "../infrastructure/change.js";
import { Client } from "../infrastructure/client.js";
import { Inventory, Item } from "../model/item.js";

export class InventoryModal {
    #modal;
    #materialRows;
    #equipmentRows;
    #inventoryChangeListener;
    #weaponChangeListener;
    #client;

    /**
     * @param {HTMLDialogElement} modal
     * @param {Client} client 
     */
    constructor(modal, client) {
        this.#modal = modal;
        this.#materialRows = document.querySelector("#materialRows");
        this.#equipmentRows = document.querySelector("#equipmentRows");
        this.#inventoryChangeListener = new ChangeListener({
            onChanged: (inventory) => this.#setInventory(inventory),
            onRemoved: () => this.#clear() 
        });
        this.#weaponChangeListener = new ChangeListener({
            onChanged: (weapon) => this.#setWeapon(weapon),
            onRemoved: () => this.#setWeapon(null)
        });
        this.#client = client;
        this.#setWeapon(null);
        this.#setCompareWeapon(null);
    }

    get inventoryChangeListener() { return this.#inventoryChangeListener; }
    get weaponChangeListener() { return this.#weaponChangeListener; }

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
            item.type.gradeName,
            item.level,
            item.description
        ].map(data => {
            const e = document.createElement("td");
            e.innerText = data;
            return e;
        });
        const tr = document.createElement("tr");
        $(tr).hover(
            () => this.#setCompareWeapon(item),
            () => this.#setCompareWeapon(null)
        );
        tds.forEach(td => tr.appendChild(td));

        const equipButton = document.createElement("button");
        equipButton.innerText = "Equip";
        equipButton.type = "button";
        equipButton.classList.add("btn");
        equipButton.classList.add("btn-primary");

        equipButton.addEventListener("click", (e) => this.#client.equip(item.id));
        
        tr.appendChild(equipButton);

        this.#equipmentRows.appendChild(tr);
    }

    /**
     * @param {Item?} weapon 
     */
    #setWeapon(weapon) {
        if (!weapon) {
            $("#noWeapon").show();
            $("#yesWeapon").hide();
            return;
        }

        $("#noWeapon").hide();
        $("#yesWeapon").show();

        this.#bindWeaponCard("#yesWeapon", weapon);
    }

    /**
     * @param {Item?} weapon 
     */
    #setCompareWeapon(weapon) {
        if (!weapon) {
            $("#compareWeapon").hide();
            return;
        }
        $("#compareWeapon").show();

        this.#bindWeaponCard("#compareWeapon", weapon);
    }

    #bindWeaponCard(selector, weapon) {
        $(selector)
            .find(".weaponName")
            .text(`${weapon.type.name} LV ${weapon.level} ${stars(weapon.type.gradeOrder)}`);
        
        $(selector)
            .find(".weaponDescription")
            .text(weapon.description);

        const details = weapon.details.map(str => {
            const e = document.createElement("li");
            e.innerText = str;
            return e;
        });
        $(selector)
            .find(".weaponDetails")
            .empty()
            .append(details);
    }
}

function stars(num) {
    let result = "";
    for (let i = 0; i < num; i++) {
        result += "*";
    }
    return result;
}

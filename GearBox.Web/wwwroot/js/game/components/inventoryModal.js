import { Client } from "../infrastructure/client.js";
import { CraftingRecipe } from "../model/crafting.js";
import { Inventory, Item } from "../model/item.js";
import { ItemDisplay } from "./itemDisplay.js";

export class InventoryModal {
    #modal;
    #materialRows;
    #recipeRows;
    #equipmentRows;
    #client;
    #craftPreview;
    #currentWeapon;
    #compareWeapon;

    /**
     * @param {HTMLDialogElement} modal
     * @param {Client} client 
     */
    constructor(modal, client) {
        this.#modal = modal;
        this.#materialRows = document.querySelector("#materialRows");
        this.#recipeRows = document.querySelector("#recipeRows");
        this.#equipmentRows = document.querySelector("#equipmentRows");
        this.#client = client;
        this.#craftPreview = new ItemDisplay("#craftPreview", "Preview", "Hover over a craft button to preview")
            .spawnHtml();
        this.#currentWeapon = new ItemDisplay("#currentWeapon", "Current Weapon", "No weapon equipped")
            .spawnHtml();
        this.#compareWeapon = new ItemDisplay("#compareWeapon", "Other Weapon", "Hover over a weapon to preview")
            .spawnHtml();

        this.setWeapon(null);
        this.#setCompareWeapon(null);
    }

    toggle() {
        if (this.#modal.open) {
            this.#modal.close();
        } else {
            this.#modal.showModal();
        }
    }

    #clearInventory() {
        this.#materialRows.replaceChildren();
        this.#equipmentRows.replaceChildren();
    }

    /**
     * @param {Inventory} inventory 
     */
    setInventory(inventory) {
        this.#clearInventory();
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
     * @param {CraftingRecipe[]} craftingRecipes 
     */
    setCraftingRecipes(craftingRecipes) {
        craftingRecipes.forEach(craftingRecipe => this.#addCraftingRecipe(craftingRecipe));
    }

    /**
     * @param {CraftingRecipe} craftingRecipe 
     */
    #addCraftingRecipe(craftingRecipe) {
        const tds = [
            craftingRecipe.makes.type.name,
            craftingRecipe.ingredients.map(i => `${i.type.name} x${i.quantity}`).join(", ")
        ].map(data => {
            const e = document.createElement("td");
            e.innerText = data;
            return e;
        });
        const tr = document.createElement("tr");
        $(tr).hover(
            () => this.#setCraftPreview(craftingRecipe.makes),
            () => this.#setCraftPreview(null)
        );
        tds.forEach(td => tr.appendChild(td));

        const craftButton = document.createElement("button");
        craftButton.innerText = "craft";
        craftButton.type = "button";
        craftButton.classList.add("btn");
        craftButton.classList.add("btn-primary");

        craftButton.addEventListener("click", (e) => this.#client.craft(craftingRecipe.id));

        tr.appendChild(craftButton);

        this.#recipeRows.appendChild(tr);
    }

    /**
     * @param {Item?} item 
     */
    #setCraftPreview(item) {
        this.#craftPreview.bind(item);
    }

    /**
     * @param {Item?} weapon 
     */
    setWeapon(weapon) {
        this.#currentWeapon.bind(weapon);
    }

    /**
     * @param {Item?} weapon 
     */
    #setCompareWeapon(weapon) {
        this.#compareWeapon.bind(weapon);
    }
}
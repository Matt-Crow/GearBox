import { Client } from "../infrastructure/client.js";
import { CraftingRecipe } from "../model/crafting.js";
import { Inventory, Item } from "../model/item.js";
import { PlayerStatSummary } from "../model/player.js";
import { EquipmentTab } from "./equipmentTab.js";
import { ItemDisplay } from "./itemDisplay.js";
import { ActionColumn, DataColumn, Table } from "./table.js";

export class InventoryModal {
    #modal;
    #materialTable;
    #recipeTable;
    #client;
    #craftPreview;
    #weaponTab;
    #armorTab;

    /**
     * @param {HTMLDialogElement} modal
     * @param {Client} client 
     */
    constructor(modal, client) {
        this.#modal = modal;

        this.#materialTable = new Table("#materials", [
            new DataColumn("Type", m => m.type.name),
            new DataColumn("Description", m => m.description),
            new DataColumn("Quantity", m => m.quantity)
        ], _ => null); // do nothing on hover
        this.#materialTable.spawnHtml();
        
        this.#recipeTable = new Table("#recipes", [
            new DataColumn("Recipe", r => r.makes.type.name),
            new DataColumn("Ingredients", r => r.ingredients.map(i => `${i.type.name} x${i.quantity}`).join(", ")),
            new ActionColumn("Action", "craft", r => this.#client.craft(r.id))
        ], r => this.#setCraftPreview(r?.makes));
        this.#recipeTable.spawnHtml();

        this.#client = client;
        this.#craftPreview = new ItemDisplay("#craftPreview", "Preview", "Hover over a craft button to preview")
            .spawnHtml();
        this.#weaponTab = new EquipmentTab("#weaponTab", id => client.equipWeapon(id));
        this.#armorTab = new EquipmentTab("#armorTab", id => client.equipArmor(id));

        this.setWeapon(null);
    }

    toggle() {
        if (this.#modal.open) {
            this.#modal.close();
        } else {
            this.#modal.showModal();
        }
    }

    /**
     * @param {Inventory} inventory 
     */
    setInventory(inventory) {
        this.#materialTable.setRecords(inventory.materials);
        this.#weaponTab.bindRows(inventory.weapons);
        this.#armorTab.bindRows(inventory.armors);
        $("#gold").text(inventory.gold);
    }

    /**
     * @param {CraftingRecipe[]} craftingRecipes 
     */
    setCraftingRecipes(craftingRecipes) {
        this.#recipeTable.setRecords(craftingRecipes);
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
        this.#weaponTab.setCurrent(weapon);
    }

    /**
     * @param {Item?} armor 
     */
    setArmor(armor) {
        this.#armorTab.setCurrent(armor);
    }

    /**
     * @param {PlayerStatSummary} statSummary 
     */
    setStatSummary(statSummary) {
        const $statsList = $("#statsList")
            .empty();
        statSummary.lines.forEach(line => $statsList.append($("<li>").text(line)));
    }
}
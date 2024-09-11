/*
    This module is responsible for the modal shown when the player opens their modal.
*/

import { Client } from "../infrastructure/client.js";
import { CraftingRecipe } from "../model/crafting.js";
import { Inventory, Item } from "../model/item.js";
import { PlayerStatSummary } from "../model/player.js";
import { OpenShop } from "../model/shop.js";
import { EquipmentTab } from "./equipmentTab.js";
import { ItemDisplay } from "./itemDisplay.js";
import { Switcher } from "./switcher.js";
import { ActionColumn, DataColumn, Table } from "./table.js";

/**
 * Shows the player's inventory or current shop
 */
export class MainModal {
    #selector;
    #element;
    #switcher;
    #client;
    #materialTable;
    #recipeTable;
    #craftPreview;
    #weaponTab;
    #armorTab;

    #currentShop = null;
    #shopBuyTable;
    #shopSellTable;
    #shopBuybackTable;
    #shopHoverInfo;

    /**
     * @param {string} selector 
     * @param {Client} client 
     */
    constructor(selector, client) {
        this.#selector = selector;
        this.#element = document.querySelector(selector);
        this.#switcher = new Switcher(selector);
        this.#client = client;

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

        this.#craftPreview = new ItemDisplay("#craftPreview", "Preview", "Hover over a craft button to preview")
            .spawnHtml();
        
        this.#weaponTab = new EquipmentTab("#weaponTab", id => client.equipWeapon(id));
        this.#armorTab = new EquipmentTab("#armorTab", id => client.equipArmor(id));

        this.#shopBuyTable = this.#makeShopTable(".shop-buy", "buy", opt => client.shopBuy(this.#currentShop?.id, opt.item.id, opt.item.type.name));
        this.#shopSellTable = this.#makeShopTable(".shop-sell", "sell", opt => client.shopSell(this.#currentShop?.id, opt.item.id, opt.item.type.name));
        this.#shopBuybackTable = this.#makeShopTable(".shop-buyback", "buy back", opt => client.shopBuy(this.#currentShop?.id, opt.item.id, opt.item.type.name));
        this.#shopHoverInfo = new ItemDisplay(`${selector} .shop-hover-info`, "Item Preview", "Hover over an item to preview it");
        this.#shopHoverInfo.spawnHtml();
    
        this.setWeapon(null);
        this.setShop(null);
    }

    /**
     * @param {string} subselector selects descendant of this modal
     * @param {string} buttonText text for the action button
     * @param {(option: OpenShopOption) => any} onButtonClick called when the user clicks the action button
     * @returns {Table}
     */
    #makeShopTable(subselector, buttonText, onButtonClick) {
        const result = new Table(`${this.#selector} ${subselector}`, [
            new DataColumn("Item", x => x.item.type.name),
            new DataColumn("Price", x => x.price),
            new ActionColumn("Action", buttonText, onButtonClick)
        ], shopOption => this.#shopHoverInfo.bind(shopOption?.item));
        result.spawnHtml();
        return result;
    }

    toggle() {
        if (this.#element.open) {
            this.#element.close();
            this.#client.closeShop();
            this.setShop(null);
        } else {
            this.#element.showModal();
        }
    }

    /**
     * @param {PlayerStatSummary} statSummary 
     */
    setStatSummary(statSummary) {
        const $statsList = $(this.#element)
            .find(".stats-list")
            .empty();
        statSummary.lines.forEach(line => $statsList.append($("<li>").text(line)));
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
     * @param {Inventory} inventory 
     */
    setInventory(inventory) {
        this.#materialTable.setRecords(inventory.materials);
        this.#weaponTab.bindRows(inventory.weapons);
        this.#armorTab.bindRows(inventory.armors);
        $("#gold").text(inventory.gold);
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
     * @param {OpenShop?} shop 
     */
    setShop(shop) {
        this.#currentShop = shop;

        if (!shop) {
            this.#switcher.show(".inventory");
            return;
        }

        this.#switcher.show(".shop");
        $(this.#element)
            .find(".shop-name")
            .text(shop.name);
        
        this.#shopBuyTable.setRecords(shop.buyOptions);
        this.#shopSellTable.setRecords(shop.sellOptions);
        this.#shopBuybackTable.setRecords(shop.buybackOptions);
    }
}
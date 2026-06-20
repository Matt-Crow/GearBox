/*
    This module is responsible for the modal shown when the player opens their modal.
*/

import { Client } from "../infrastructure/client.js";
import { MaybeChange, UiStateChanges } from "../model/areaUpdate.js";
import { CraftingRecipe } from "../model/crafting.js";
import { EQUIPMENT_SLOT_TYPES, Inventory } from "../model/item.js";
import { PassiveAbility } from "../model/passiveAbility.js";
import { PlayerStatSummary } from "../model/player.js";
import { OpenShop } from "../model/shop.js";
import { EquipmentTab } from "./equipmentTab.js";
import { ItemDisplay } from "./itemDisplay.js";
import { Switcher } from "./shared/switcher.js";
import { ActionColumn, ConditionalActionColumn, DataColumn, Table } from "./table.js";

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
    #equipmentTabs;

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
            new DataColumn("Type", m => m.name),
            new DataColumn("Description", m => m.description),
            new DataColumn("Quantity", m => m.quantity)
        ]);
        this.#materialTable.spawnHtml();

        this.#craftPreview = new ItemDisplay("#craft-preview", "Preview")
            .spawnHtml()
            .hide();

        this.#recipeTable = new Table("#recipes", [
            new DataColumn("Recipe", r => r.makes.name),
            new DataColumn("Ingredients", r => r.ingredients.map(i => `${i.name} x${i.quantity}`).join(", ")),
            new ActionColumn("Action", "craft", r => this.#client.craft(r.id))
        ], (record, row) => this.#craftPreview.handleRowCreated(record?.makes, row));
        this.#recipeTable.spawnHtml();
        
        this.#shopBuyTable = this.#makeShopTable(".shop-buy", "buy", opt => client.shopBuy(this.#currentShop?.id, opt.item.id, opt.item.name));
        this.#shopSellTable = this.#makeShopTable(".shop-sell", "sell", opt => client.shopSell(this.#currentShop?.id, opt.item.id, opt.item.name));
        this.#shopBuybackTable = this.#makeShopTable(".shop-buyback", "buy back", opt => client.shopBuy(this.#currentShop?.id, opt.item.id, opt.item.name));
        this.#shopHoverInfo = new ItemDisplay(`${selector} .shop-preview`, "Item Preview");
        this.#shopHoverInfo
            .spawnHtml()
            .hide();
    
        this.#setShop(null);

        this.#equipmentTabs = new Map();
        this.#equipmentTabs.set(EQUIPMENT_SLOT_TYPES.LOCOMOTION, new EquipmentTab("#locomotionTab", id => client.equip(id)));
        this.#equipmentTabs.set(EQUIPMENT_SLOT_TYPES.MANIPULATOR, new EquipmentTab("#manipulatorTab", id => client.equip(id)));
        this.#equipmentTabs.set(EQUIPMENT_SLOT_TYPES.TORSO, new EquipmentTab("#torsoTab", id => client.equip(id)));
    }

    /**
     * @param {string} subselector selects descendant of this modal
     * @param {string} buttonText text for the action button
     * @param {(option: OpenShopOption) => any} onButtonClick called when the user clicks the action button
     * @returns {Table}
     */
    #makeShopTable(subselector, buttonText, onButtonClick) {
        const result = new Table(`${this.#selector} ${subselector}`, [
            new DataColumn("Item", x => x.item.name),
            new DataColumn("Price", x => x.price),
            new ConditionalActionColumn("Action", buttonText, onButtonClick, i => i.canAfford)
        ], (record, row) => this.#shopHoverInfo.handleRowCreated(record?.item, row));
        result.spawnHtml();
        return result;
    }

    toggle() {
        if (this.#element.open) {
            this.#element.close();
            this.#client.closeShop();
            this.#setShop(null);
        } else {
            this.#element.showModal();
        }
    }

    /**
     * @param {UiStateChanges} uiStateChanges 
     */
    handleUiStateChanges(uiStateChanges) {
        Object.values(EQUIPMENT_SLOT_TYPES).forEach(slotType => {
            const maybeChange = uiStateChanges.equipment.get(slotType);
            const tab = this.#equipmentTabs.get(slotType);
            maybeChange.handleChange(e => tab.setCurrent(e));
        });
        uiStateChanges.inventory.handleChange(i => this.setInventory(i));
        uiStateChanges.summary.handleChange(s => this.setStatSummary(s));
        uiStateChanges.passives.handleChange(p => this.setPassives(p));
        uiStateChanges.openShop.handleChange(os => this.#setShop(os));
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
     * @param {PassiveAbility[]} passives 
     */
    setPassives(passives) {
        const $passiveList = $(this.#element)
            .find(".passive-list")
            .empty();
        passives.forEach(passive => $passiveList.append($("<li>")
            .append($("<b>").text(passive.name + ": "))
            .append(passive.description)
        ));
    }

    /**
     * @param {CraftingRecipe[]} craftingRecipes 
     */
    setCraftingRecipes(craftingRecipes) {
        this.#recipeTable.setRecords(craftingRecipes);
    }

    /**
     * @param {Inventory} inventory 
     */
    setInventory(inventory) {
        for (const [slotType, tab] of this.#equipmentTabs.entries()) {
            const equipmentForThisTab = inventory.equipment.filter(e => e.slotType === slotType);
            tab.bindRows(equipmentForThisTab);
        }
        this.#materialTable.setRecords(inventory.materials);
        $("#gold").text(inventory.gold);
    }

    /**
     * @param {OpenShop?} shop 
     */
    #setShop(shop) {
        this.#currentShop = shop;

        if (!shop) {
            this.#switcher.show(".inventory");
            return;
        }

        this.#switcher.show(".shop");
        $(this.#element)
            .find(".shop-name")
            .text(shop.name);
        
        $(this.#element)
            .find(".player-gold")
            .text(shop.playerGold);
        
        this.#shopBuyTable.setRecords(shop.buyOptions);
        this.#shopSellTable.setRecords(shop.sellOptions);
        this.#shopBuybackTable.setRecords(shop.buybackOptions);
    }
}
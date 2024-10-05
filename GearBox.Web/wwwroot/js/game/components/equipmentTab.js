import { Item } from "../model/item.js";
import { ItemDisplay } from "./itemDisplay.js";
import { ActionColumn, DataColumn, Table } from "./table.js";

export class EquipmentTab {
    #selector;
    #table;
    #onEquip;
    #currentEquipment;
    #compareEquipment;

    /**
     * 
     * @param {string} selector 
     * @param {(string) => any} onEquip 
     */
    constructor(selector, onEquip) {
        this.#selector = selector;
        this.#onEquip = onEquip;
        this.#table = new Table(`${this.#selector} .equipment-table`, [
            new DataColumn("Name", e => e.name),
            new DataColumn("Grade", e => e.gradeName),
            new DataColumn("Level", e => e.level),
            new DataColumn("Description", e => e.description),
            new ActionColumn("Action", "Equip", e => this.#onEquip(e.id))
        ], (record, row) => this.#compareEquipment.handleRowCreated(record, row));
        this.#spawnHtml();
        this.#currentEquipment = new ItemDisplay(`${selector} .current-equipment`, "Current equipment", "Nothing equipped")
            .spawnHtml();
        this.#compareEquipment = new ItemDisplay(`${selector} .compare-equipment`, "Other equipment")
            .spawnHtml();
        this.#setCompare(null);
    }

    #spawnHtml() {
        $(this.#selector)
            .empty()
            .append($("<div>").addClass("table-responsive").addClass("h-50")
                .append($("<div>").addClass("equipment-table"))
            )
            .append($("<div>").addClass("h-50").addClass("container")
                .append($("<div>").addClass("row")
                    .append($("<div>").addClass("col-6").addClass("current-equipment"))
                    .append($("<div>").addClass("col-6").addClass("compare-equipment gb-tooltip"))
                )
            );
        this.#table.spawnHtml();
    }

    /**
     * @param {Item[]} items 
     */
    bindRows(items) {
        this.#table.setRecords(items);
    }

    setCurrent(item) {
        this.#currentEquipment.bind(item);
    }

    #setCompare(item) {
        this.#compareEquipment.bind(item);
    }
}
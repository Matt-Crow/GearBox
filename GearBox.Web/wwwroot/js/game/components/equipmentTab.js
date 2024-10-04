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
        this.#table = new Table(`${this.#selector} .equipmentTable`, [
            new DataColumn("Name", e => e.name),
            new DataColumn("Grade", e => e.gradeName),
            new DataColumn("Level", e => e.level),
            new DataColumn("Description", e => e.description),
            new ActionColumn("Action", "Equip", e => this.#onEquip(e.id))
        ], (record, row) => {
            row.addEventListener("mouseenter", _ => this.#setCompare(record));
            row.addEventListener("mouseleave", _ => this.#setCompare(null));
        });
        this.#spawnHtml();
        this.#currentEquipment = new ItemDisplay(`${selector} .currentEquipment`, "Current equipment", "Nothing equipped")
            .spawnHtml();
        this.#compareEquipment = new ItemDisplay(`${selector} .compareEquipment`, "Other equipment", "Hover over a row to compare")
            .spawnHtml();
        this.#setCompare(null);
    }

    #spawnHtml() {
        $(this.#selector)
            .empty()
            .append($("<div>").addClass("table-responsive").addClass("h-50")
                .append($("<div>").addClass("equipmentTable"))
            )
            .append($("<div>").addClass("h-50").addClass("container")
                .append($("<div>").addClass("row")
                    .append($("<div>").addClass("col-6").addClass("currentEquipment"))
                    .append($("<div>").addClass("col-6").addClass("compareEquipment"))
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
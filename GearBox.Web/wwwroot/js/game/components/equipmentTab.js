import { Item } from "../model/item.js";
import { ItemDisplay } from "./itemDisplay.js";
import { ActionColumn, DataColumn, Table } from "./table.js";

export class EquipmentTab {
    #selector;
    #table;
    #onEquip;
    #currentEquipment;
    #compareCurrent;
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
            new ActionColumn("Action", "Equip", e => this.#onEquip(e.id))
        ], (record, row) => {
            row.addEventListener("mouseenter", _ => {
                this.#setCompare(record);
                $(`${selector} .compare`).show();
            });
            row.addEventListener("mouseleave", _ => {
                this.#setCompare(null);
                $(`${selector} .compare`).hide();
            });
            row.addEventListener("mousemove", e => {
                /*
                    e.client_ is relative to viewport
                    css left is relative to containing block
                    so need to convert e.client_ to relative to containing block
                    https://developer.mozilla.org/en-US/docs/Web/CSS/left#:~:text=for%20absolutely%20positioned%20elements%2C%20the%20distance%20to%20the%20left%20edge%20of%20the%20containing%20block
                */
                const containingBlock = $(e.target).offsetParent();
                const blockCoords = containingBlock[0].getBoundingClientRect();
                $(`${selector} .compare`)
                    .css({
                        left: e.clientX - blockCoords.left + 25,
                        top: e.clientY - blockCoords.top + 25
                    });
                    //.show();
            });
        });
        this.#spawnHtml();
        this.#currentEquipment = new ItemDisplay(`${selector} .current-equipment`, "Current equipment", "Nothing equipped")
            .spawnHtml();
        this.#compareCurrent = new ItemDisplay(`${selector} .compare-current`, "Current")
            .spawnHtml();
        this.#compareEquipment = new ItemDisplay(`${selector} .compare-equipment`, "Other")
            .spawnHtml();
        this.#setCompare(null);
    }

    #spawnHtml() {
        $(this.#selector)
            .empty()
            .append($("<div>").addClass("row")
                .append($("<div>").addClass("table-responsive").addClass("col-8")
                    .append($("<div>").addClass("equipment-table"))
                )
                .append($("<div>").addClass("h-50").addClass("col-4")
                    .append($("<div>").addClass("current-equipment"))
                )
            )
            .append($("<div>").addClass("gb-tooltip compare row")
                .append($("<div>").addClass("compare-current col"))
                .append($("<div>").addClass("compare-equipment col"))
                .hide()
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
        this.#compareCurrent.bind(item);
        if (item) {
            this.#compareCurrent.show();
        } else {
            this.#compareCurrent.hide();
        }
    }

    #setCompare(item) {
        this.#compareEquipment.bind(item);
    }
}
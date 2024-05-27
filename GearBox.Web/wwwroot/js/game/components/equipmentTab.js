import { Item } from "../model/item.js";
import { ItemDisplay } from "./itemDisplay.js";

export class EquipmentTab
{
    #selector;
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
                .append($("<table>").addClass("table").addClass("table-striped").addClass("table-hove")
                    .append($("<thead>").addClass("thead-dark")
                        .append($("<tr>")
                            .append($("<td>").text("Name"))
                            .append($("<td>").text("Grade"))
                            .append($("<td>").text("Level"))
                            .append($("<td>").text("Description"))
                            .append($("<td>").text("Action"))
                        )
                    )
                    .append($("<tbody>").addClass("equipmentRows"))
                )
            )
            .append($("<div>").addClass("h-50").addClass("container")
                .append($("<div>").addClass("row")
                    .append($("<div>").addClass("col-6").addClass("currentEquipment"))
                    .append($("<div>").addClass("col-6").addClass("compareEquipment"))
                )
            );
    }

    /**
     * @param {Item[]} items 
     */
    bindRows(items) {
        $(this.#selector)
            .find(".equipmentRows")
            .empty();
        items.forEach(item => this.#addItem(item));
    }

    /**
     * @param {Item} item 
     */
    #addItem(item) {
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
            () => this.#setCompare(item),
            () => this.#setCompare(null)
        );
        tds.forEach(td => tr.appendChild(td));

        const equipButton = document.createElement("button");
        equipButton.innerText = "Equip";
        equipButton.type = "button";
        equipButton.classList.add("btn");
        equipButton.classList.add("btn-primary");

        equipButton.addEventListener("click", (_) => this.#onEquip(item.id));

        tr.appendChild(equipButton);

        $(this.#selector)
            .find(".equipmentRows")
            .append(tr);
    }

    setCurrent(item) {
        this.#currentEquipment.bind(item);
    }

    #setCompare(item) {
        this.#compareEquipment.bind(item);
    }
}
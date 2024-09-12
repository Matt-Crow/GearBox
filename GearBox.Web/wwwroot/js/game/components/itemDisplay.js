import { Item } from "../model/item.js";

export class ItemDisplay {
    #selector;
    #title;
    #emptyText;

    /**
     * @param {string} selector 
     * @param {string} title 
     * @param {string} [emptyText=""] 
     */
    constructor(selector, title, emptyText="") {
        this.#selector = selector;
        this.#title = title;
        this.#emptyText = emptyText;
    }

    /**
     * @returns {ItemDisplay}
     */
    spawnHtml() {
        const $this = $(this.#selector)
            .empty()
            .append($("<div>").addClass("itemYes")
                .append($("<h2>").text(this.#title))
                .append($("<p>").append($("<b>").addClass("itemName")))
                .append($("<p>").append($("<i>").addClass("itemDescription")))
                .append($("<ul>").addClass("itemDetails"))
            )
            .append($("<div>").addClass("itemNo")
                .text(this.#emptyText)
            );
        return this;
    }

    /**
     * @param {Item?} item 
     */
    bind(item) {
        const $this = $(this.#selector);
        if (!item) {
            $this.find(".itemYes").hide();
            $this.find(".itemNo").show();
            return;
        }

        $this.find(".itemYes").show();
        $this.find(".itemNo").hide();

        $this
            .find(".itemName")
            .text(`${item.name} LV ${item.level} ${stars(item.gradeOrder)}`);
        
        $this
            .find(".itemDescription")
            .text(item.description);

        const details = item.details.map(str => $("<li>").text(str));
        $this
            .find(".itemDetails")
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
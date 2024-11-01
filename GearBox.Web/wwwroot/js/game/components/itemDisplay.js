import { ActiveAbility } from "../model/activeAbility.js";
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
                .append($("<p>").append($("<b>").addClass("itemName text-nowrap")))
                .append($("<p>").append($("<i>").addClass("itemDescription")))
                .append($("<ul>").addClass("itemDetails"))
                .append($("<div>").addClass("itemActives")
                    .append($("<hr>"))
                    .append($("<b>").append($("<u>").text("Active Abilities")))
                    .append($("<div>").addClass("itemActiveList"))
                )
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
        
        if (item.actives.length === 0) {
            $this.find(".itemActives").hide();
        } else {
            $this.find(".itemActives").show();
            const abilities = item.actives.map(act => this.#makeActiveDisplay(act));
            $this
                .find(".itemActiveList")
                .empty()
                .append(abilities);
        }
    }

    /**
     * @param {ActiveAbility} active 
     * @returns a JQuery object
     */
    #makeActiveDisplay(active) {
        const $result = $("<div>")
            .append($("<b>").text(active.name))
            .append($("<span>").text(`(${active.energyCost} energy)`))
            .append($("<p>").addClass("text-wrap").css("width", "20rem").text(active.description));
        return $result;
    }

    show() {
        $(this.#selector).show();
        return this;
    }

    hide() {
        $(this.#selector).hide();
        return this;
    }

    handleRowCreated(record, row) {
        row.addEventListener("mouseenter", _ => {
            this.bind(record);
            $(this.#selector).show();
        });
        row.addEventListener("mouseleave", _ => {
            this.bind(null);
            $(this.#selector).hide();
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
            $(this.#selector)
                .css({
                    left: e.clientX - blockCoords.left + 25,
                    top: e.clientY - blockCoords.top + 25
                })
                .show();
        });
    }
}

function stars(num) {
    let result = "";
    for (let i = 0; i < num; i++) {
        result += "*";
    }
    return result;
}
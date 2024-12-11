import { ActiveAbility } from "../../../model/activeAbility.js";

export class ActiveAbilityHud {
    #selector;
    #number;

    constructor(selector, number) {
        this.#selector = selector;
        this.#number = number;
    }

    spawnHtml() {
        $(this.#selector)
            .empty()
            .append($("<div>").addClass("d-flex flex-column text-center").css("border", "1em ridge lightblue")
                .append($("<p>")
                    .append($("<span>").text(`[${this.#number}]`))
                    .append($("<br/>"))
                    .append($("<span>").addClass("active-yes active-name"))
                    .append($("<br/>"))
                    .append($("<span>").addClass("active-yes active-status"))
                )
        );
    }

    /**
     * @param {ActiveAbility?} active 
     */
    setActive(active) {
        const $activeYes = $(`${this.#selector} .active-yes`);
        if (!active) {
            $activeYes.hide();
            return;
        }
        $activeYes.show();

        const status = active.secondsUntilNextUse > 0 
            ? `${active.secondsUntilNextUse}s`
            : `${active.energyCost} energy`;

        $(this.#selector).find(".active-name").text(active.name);
        $(this.#selector).find(".active-status").text(status);
    }
}
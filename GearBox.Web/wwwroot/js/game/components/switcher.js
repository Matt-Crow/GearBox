/*
    Switches between which of its children are visible
*/
export class Switcher {
    #selector;

    /**
     * @param {string} selector 
     */
    constructor(selector) {
        this.#selector = selector;
        this.hideAll();
    }

    /**
     * hides all children
     */
    hideAll() {
        $(this.#selector)
            .children()
            .hide();
    }

    /**
     * @param {string} selector the selector for children to show
     */
    show(selector) {
        this.hideAll();
        $(this.#selector)
            .children(selector)
            .show();
    }
}
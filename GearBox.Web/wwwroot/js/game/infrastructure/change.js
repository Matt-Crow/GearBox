/**
 * This module is responsible for handling changes to stable game objects
 * emitted by the server.
 */

export class ChangeListener {
    #onChanged;
    #onRemoved;

    constructor({onChanged, onRemoved}) {
        const doNothing = () => {};
        this.#onChanged = onChanged ?? doNothing;
        this.#onRemoved = onRemoved ?? doNothing;
    }

    changed(obj) {
        this.#onChanged(obj);
    }

    removed(obj) {
        this.#onRemoved(obj);
    }
}
/**
 * This module is responsible for handling changes to stable game objects
 * emitted by the server.
 */

export class ChangeHandlers {
    #handlers = new Map();

    /**
     * @param {ChangeHandler} handler 
     * @returns {ChangeHandlers}
     */
    withChangeHandler(handler) {
        this.#handlers.set(handler.bodyType, handler);
        return this;
    }

    /**
     * @param {Change} change 
     */
    handle(change) {
        const bodyType = change.bodyType;
        if (!this.#handlers.has(bodyType)) {
            throw new Error(`Unsupported change body type: "${bodyType}"`);
        }

        const obj = JSON.parse(change.body);
        const handler = this.#handlers.get(bodyType);
        if (change.isDelete) {
            handler.handleDelete(obj);
        } else {
            handler.handleContent(obj);
        }
    }
}

export class ChangeHandler {
    #bodyType;

    constructor(bodyType) {
        this.#bodyType = bodyType;
    }

    /**
     * @returns {string}
     */
    get bodyType() {
        return this.#bodyType;
    }

    /**
     * @param {object} body 
     */
    handleContent(_body) {
        throw new Error("abstract method");
    }

    /**
     * @param {object} body 
     */
    handleDelete(_body) {
        throw new Error("abstract method");
    }
}

export class Change {
    #bodyType;
    #body;
    #isDelete;

    constructor(bodyType, body, isDelete) {
        this.#bodyType = bodyType;
        this.#body = body;
        this.#isDelete = isDelete;
    }

    static fromJson(json) {
        return new Change(json.bodyType, json.body, json.isDelete);
    }

    /**
     * @returns {string}
     */
    get bodyType() {
        return this.#bodyType;
    }

    /**
     * @returns {string}
     */
    get body() {
        return this.#body;
    }

    /**
     * @returns {boolean}
     */
    get isDelete() {
        return this.#isDelete;
    }
}

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
/**
 * This module is responsible for handling changes to stable game objects
 * emitted by the server.
 */

export class ChangeHandlers {
    #handlers = new Map();

    add(handler) {
        this.#handlers.set(handler.bodyType, handler);
    }

    handle(change) {
        const bodyType = change.bodyType;
        if (!this.#handlers.has(bodyType)) {
            throw new Error(`Unsupported change body type: "${bodyType}"`);
        }

        const handler = this.#handlers.get(bodyType);
        switch (change.changeType.toLowerCase()) {
            case "create": {
                handler.handleCreate(change.body);
                break;
            }
            case "update": {
                handler.handleUpdate(change.body);
                break;
            }
            case "delete": {
                handler.handleDelete(change.body);
                break;
            }
            default: {
                throw new Error(`Unsupported change type: "${change.changeType}"`);
            }
        }
    }
}

export class ChangeHandler {
    #bodyType;

    constructor(bodyType) {
        this.#bodyType = bodyType;
    }

    get bodyType() {
        return this.#bodyType;
    }

    /**
     * @param {string} body 
     */
    handleCreate(body) {
        throw new Error("abstract method");
    }

    /**
     * @param {string} body 
     */
    handleUpdate(body) {
        throw new Error("abstract method");
    }

    /**
     * @param {string} body 
     */
    handleDelete(body) {
        throw new Error("abstract method");
    }
}

export class Change {
    #changeType;
    #bodyType;
    #body;

    constructor(changeType, bodyType, body) {
        this.#changeType = changeType;
        this.#bodyType = bodyType;
        this.#body = body;
    }

    static fromJson(json) {
        return new Change(json.changeType, json.bodyType, json.body);
    }

    get changeType() {
        return this.#changeType;
    }

    get bodyType() {
        return this.#bodyType;
    }

    get body() {
        return this.#body;
    }
}
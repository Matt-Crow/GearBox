import { MessageHandler } from "./messageHandler.js";

export class MessageHandlers {
    #handlers;

    constructor() {
        this.#handlers = new Map();
    }

    /**
     * @param {MessageHandler} handler
     */
    addHandler(handler) {
        this.#handlers.set(handler.type, handler);
    }

    /**
     * @param {object} message the JSON object received through SignalR
     */
    handle(message) {
        if (this.#handlers.has(message.type)) {
            this.#handlers.get(message.type).handle(message.body);
        } else {
            throw new Error(`Unsupported message type: ${message.type}`);
        }
    }
}
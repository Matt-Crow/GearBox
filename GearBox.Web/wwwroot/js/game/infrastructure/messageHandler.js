/**
 * deserializes messages received through SignalR into JavaScript objects.
 */
export class MessageHandler {
    #type;
    #handler;

    constructor(type, handler) {
        this.#type = type;
        this.#handler = handler;
    }

    /**
     * @returns {string} the type of message this can handle
     */
    get type() {
        return this.#type;
    }

    /**
     * 
     * @param {object} messageBody the JSON body to handle
     */
    handle(messageBody) {
        this.#handler(messageBody);
    }
}
import { MessageHandlers } from "./infrastructure/messageHandlers.js";
import { CharacterJsonDeserializer } from "./model/character.js";
import { WorldDeserializer, WorldInitHandler, WorldProxy, WorldUpdateHandler } from "./model/world.js";


export class Game {
    /**
     * The HTMLCanvasElement this draws on.
     */
    #canvas;

    /**
     * A WorldProxy which can be used to access the current world - if any.
     */
    #worldProxy = new WorldProxy();

    /**
     * MessageHandlers which deserialize messages received from the server
     */
    #messageHandlers = new MessageHandlers();

    /**
     * @param {HTMLCanvasElement} canvas the HTML canvas to draw on.
     */
    constructor(canvas) {
        this.#canvas = canvas;

        const worldDeserializer = new WorldDeserializer();
        worldDeserializer.addDynamicObjectDeserializer(new CharacterJsonDeserializer());

        this.#messageHandlers.addHandler(new WorldInitHandler(this.#worldProxy, worldDeserializer));
        this.#messageHandlers.addHandler(new WorldUpdateHandler(this.#worldProxy, worldDeserializer));

        setInterval(() => this.#update(), 1000); // todo better frame rate
    }

    /**
     * Canned by SignalR to process messages
     * @param {object} message a JSON message received through SignalR
     */
    handle(message) {
        this.#messageHandlers.handle(message);
    }

    #update() {
        try {
            const world = this.#worldProxy.value;
            world.draw(this.#canvas.getContext("2d"));
        } catch (e) {
            // world is not yet ready; don't bother reporting, as update handler does so
        }
    }
}
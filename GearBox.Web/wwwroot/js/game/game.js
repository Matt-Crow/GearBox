import { JsonDeserializers } from "./infrastructure/jsonDeserializers.js";
import { MessageHandlers } from "./infrastructure/messageHandlers.js";
import { CharacterJsonDeserializer } from "./model/character.js";
import { WorldDeserializer, WorldInitHandler, WorldProxy, WorldUpdateHandler } from "./model/world.js";

export class Game {
    #canvas;
    #worldProxy;
    #jsonDeserializers;
    #messageHandlers;

    /**
     * @param {HTMLCanvasElement} canvas the HTML canvas to draw on.
     */
    constructor(canvas) {
        this.#canvas = canvas;
        this.#worldProxy = new WorldProxy();
        
        this.#jsonDeserializers = new JsonDeserializers();
        this.#jsonDeserializers.addDeserializer(new CharacterJsonDeserializer());

        const worldDeserializer = new WorldDeserializer();

        this.#messageHandlers = new MessageHandlers();
        this.#messageHandlers.addHandler(new WorldInitHandler(this.#worldProxy, worldDeserializer));
        this.#messageHandlers.addHandler(new WorldUpdateHandler(this.#worldProxy, this.#jsonDeserializers));

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
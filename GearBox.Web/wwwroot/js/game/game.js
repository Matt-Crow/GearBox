import { JsonDeserializers } from "./infrastructure/jsonDeserializers.js";
import { MessageHandlers } from "./infrastructure/messageHandlers.js";
import { CharacterJsonDeserializer } from "./model/character.js";
import { WorldInitHandler, WorldProxy, WorldUpdateHandler } from "./model/world.js";

export class Game {
    #worldProxy;
    #jsonDeserializers;
    #messageHandlers;

    constructor() {
        this.#worldProxy = new WorldProxy();
        
        this.#jsonDeserializers = new JsonDeserializers();
        this.#jsonDeserializers.addDeserializer(new CharacterJsonDeserializer());

        this.#messageHandlers = new MessageHandlers();
        this.#messageHandlers.addHandler(new WorldInitHandler(this.#worldProxy, this.#jsonDeserializers));
        this.#messageHandlers.addHandler(new WorldUpdateHandler(this.#worldProxy, this.#jsonDeserializers));
    }

    /**
     * @param {object} message a JSON message received through SignalR
     */
    handle(message) {
        this.#messageHandlers.handle(message);
    }
}
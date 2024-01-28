import { ChangeHandlers } from "./infrastructure/change.js";
import { MessageHandlers } from "./infrastructure/messageHandlers.js";
import { CharacterJsonDeserializer } from "./model/character.js";
import { ItemChangeHandler } from "./model/item.js";
import { PlayerChangeHandler } from "./model/player.js";
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

        const changeHandlers = new ChangeHandlers();
        changeHandlers.add(new ItemChangeHandler());
        changeHandlers.add(new PlayerChangeHandler());

        const worldDeserializer = new WorldDeserializer(changeHandlers);
        worldDeserializer.addDynamicObjectDeserializer(new CharacterJsonDeserializer());

        this.#messageHandlers.addHandler(new WorldInitHandler(this.#worldProxy, worldDeserializer));
        this.#messageHandlers.addHandler(new WorldUpdateHandler(this.#worldProxy, worldDeserializer));

        setInterval(() => this.#update(), 1000 / 24);
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
            const player = world.player;
            const w = this.#canvas.width;
            const h = this.#canvas.height;
            const ctx = this.#canvas.getContext("2d");
            ctx.clearRect(0, 0, w, h);
            if (player) {
                ctx.translate(
                    clamp(w - world.widthInPixels, -player.x + w/2, 0), 
                    clamp(h - world.heightInPixels, -player.y + h/2, 0)
                );
            }
            world.draw(ctx);
            ctx.resetTransform();
        } catch (e) {
            // world is not yet ready; don't bother reporting, as update handler does so
        }
    }
}

function clamp(min, x, max) {
    if (x > max) {
        return max;
    }
    if (x < min) {
        return min;
    }
    return x;
}
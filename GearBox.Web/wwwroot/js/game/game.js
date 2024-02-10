import { InventoryModal } from "./components/inventory.js";
import { ChangeHandlers } from "./infrastructure/change.js";
import { CharacterJsonDeserializer } from "./model/character.js";
import { InventoryDeserializer, ItemChangeHandler, ItemDeserializer } from "./model/item.js";
import { LootChestChangeHandler } from "./model/lootChest.js";
import { PlayerChangeHandler, PlayerDeserializer, PlayerRepository } from "./model/player.js";
import { WorldInitHandler, WorldUpdateHandler } from "./model/world.js";

export class Game {
    /**
     * The HTMLCanvasElement this draws on.
     */
    #canvas;

    /**
     * The InventoryModal for the current player.
     */
    #inventoryModal;

    /**
     * The current function for handling messages from the server.
     * Currently, this assumes the server will always start by sending WorldInitJson,
     * followed by an number of WorldUpdates,
     * and no other message types.
     */
    #handleMessage;

    /**
     * @param {HTMLCanvasElement} canvas the HTML canvas to draw on.
     * @param {InventoryModal} inventoryModal the modal for the current player's inventory.
     */
    constructor(canvas, inventoryModal) {
        this.#canvas = canvas;
        this.#inventoryModal = inventoryModal;
        this.#handleMessage = (message) => this.#handleInit(message);
    }

    /**
     * Called by SignalR to process messages.
     * @param {object} message a JSON message received through SignalR
     */
    handle(message) {
        this.#handleMessage(message); 
    }

    #handleInit(initMessage) {
        const world = new WorldInitHandler()
            .handleWorldInit(initMessage);

        // set up event listeners to update inventory modal when player changes
        const players = new PlayerRepository();
        players.addPlayerListener(world.playerId, this.#inventoryModal.playerEventListener);
        
        const itemDeserializer = new ItemDeserializer(world.itemTypes);
        const changeHandlers = new ChangeHandlers();
        changeHandlers.add(new ItemChangeHandler());
        changeHandlers.add(new LootChestChangeHandler(world));
        changeHandlers.add(new PlayerChangeHandler(players, new PlayerDeserializer(new InventoryDeserializer(itemDeserializer))));
        const updateHandler = new WorldUpdateHandler(world, changeHandlers)
            .withDynamicObjectDeserializer(new CharacterJsonDeserializer());

        // unregisters handleInit, switches to handling updates instead
        this.#handleMessage = (updateMessage) => updateHandler.handleWorldUpdate(updateMessage);
        
        setInterval(() => this.#update(world), 1000 / 24);
    }

    #update(world) {
        const player = world.player;
        const w = this.#canvas.width;
        const h = this.#canvas.height;
        const ctx = this.#canvas.getContext("2d");
        ctx.clearRect(0, 0, w, h);
        if (player) {
            ctx.translate(
                clamp(w - world.widthInPixels, w/2 - player.x, 0), 
                clamp(h - world.heightInPixels, h/2 - player.y, 0)
            );
        }
        world.draw(ctx);
        ctx.resetTransform();
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
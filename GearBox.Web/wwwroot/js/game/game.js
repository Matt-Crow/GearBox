import { ChangeHandlers } from "./infrastructure/change.js";
import { MessageHandlers } from "./infrastructure/messageHandlers.js";
import { CharacterJsonDeserializer } from "./model/character.js";
import { Inventory, InventoryDeserializer, Item, ItemChangeHandler, ItemDeserializer } from "./model/item.js";
import { PlayerChangeHandler, PlayerDeserializer, PlayerEventListener, PlayerRepository } from "./model/player.js";
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
     * @param {HTMLTableSectionElement} inventoryRows the <tbody> of the inventory model.
     */
    constructor(canvas, inventoryRows) {
        this.#canvas = canvas;

        const changeHandlers = new ChangeHandlers();
        changeHandlers.add(new ItemChangeHandler());
        const players = new PlayerRepository();

        // item type repository is not yet loaded, as the world hasn't been received.
        // therefore, lazy load the repo until I split phases in #34
        const itemDeserializer = new ItemDeserializer(() => this.#worldProxy.value.itemTypes);
        
        const inventoryDeserializer = new InventoryDeserializer(itemDeserializer);
        const playerDeserializer = new PlayerDeserializer(inventoryDeserializer);
        changeHandlers.add(new PlayerChangeHandler(players, playerDeserializer));

        const worldDeserializer = new WorldDeserializer(changeHandlers);
        worldDeserializer.addDynamicObjectDeserializer(new CharacterJsonDeserializer());

        // todo clean this up after #34
        this.#messageHandlers.addHandler(new WorldInitHandler(this.#worldProxy, worldDeserializer, () => {
            const inventoryModal = new InventoryModal(inventoryRows);
            players.addPlayerListener(this.#worldProxy.value.playerId, new PlayerEventListener({
                onPlayerAdded: (player) => inventoryModal.setInventory(player.inventory),
                onPlayerUpdated: (player) => inventoryModal.setInventory(player.inventory),
                onPlayerRemoved: () => inventoryModal.clear()
            }));
        }));
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

class InventoryModal {
    #tbody;

    /**
     * @param {HTMLTableSectionElement} tbody 
     */
    constructor(tbody) {
        this.#tbody = tbody;
    }

    clear() {
        this.#tbody.replaceChildren();
    }

    /**
     * @param {Inventory} inventory 
     */
    setInventory(inventory) {
        this.clear();
        inventory.equipment.forEach(item => this.#addItem(item));
        inventory.materials.forEach(item => this.#addItem(item));
    }

    /**
     * @param {Item} item 
     */
    #addItem(item) {
        const tds = [
            item.type.name,
            item.type.description ?? "items don't have descriptions yet",
            item.quantity
        ].map(data => {
            const e = document.createElement("td");
            e.innerText = data;
            return e;
        });
        const tr = document.createElement("tr");
        tds.forEach(td => tr.appendChild(td));
        this.#tbody.appendChild(tr);
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
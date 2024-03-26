import { Canvas } from "./components/canvas.js";
import { InventoryModal } from "./components/inventoryModal.js";
import { PlayerHud } from "./components/playerHud.js";
import { ChangeHandlers } from "./infrastructure/change.js";
import { CharacterJsonDeserializer } from "./model/character.js";
import { EquippedWeaponChangeHandler, InventoryChangeHandler, InventoryDeserializer, ItemDeserializer } from "./model/item.js";
import { LootChestChangeHandler } from "./model/lootChest.js";
import { PlayerChangeHandler, PlayerDeserializer, PlayerRepository } from "./model/player.js";
import { ProjectileJsonDeserializer } from "./model/projectile.js";
import { WorldInitHandler, WorldUpdateHandler } from "./model/world.js";

export class Game {

    /**
     * The custom canvas component this draws on.
     */
    #canvas;

    /**
     * The InventoryModal for the current player.
     */
    #inventoryModal;

    /**
     * The PlayerHud for the current player.
     */
    #playerHud;

    /**
     * The current function for handling messages from the server.
     * Currently, this assumes the server will always start by sending WorldInitJson,
     * followed by an number of WorldUpdates,
     * and no other message types.
     */
    #handleMessage;

    #world; // required for getting mouse cursor position relative to player :(

    /**
     * @param {Canvas} canvas the custom canvas component to draw on.
     * @param {InventoryModal} inventoryModal the modal for the current player's inventory.
     * @param {PlayerHud} playerHud the player HUD for the current player
     */
    constructor(canvas, inventoryModal, playerHud) {
        this.#canvas = canvas
        this.#inventoryModal = inventoryModal;
        this.#playerHud = playerHud;
        this.#handleMessage = (message) => this.#handleInit(message);
        this.#world = null;
    }

    /**
     * Called by SignalR to process messages.
     * @param {object} message a JSON message received through SignalR
     */
    handle(message) {
        this.#handleMessage(message); 
    }

    getPlayerCoords() {
        const player = this.#world?.player;
        return [player?.x, player?.y];
    }

    #handleInit(initMessage) {
        const world = new WorldInitHandler()
            .handleWorldInit(initMessage);

        // set up event listeners to update UI when player changes
        const players = new PlayerRepository();
        players.addPlayerListener(world.playerId, this.#playerHud.playerEventListener);
        this.#playerHud.characterSupplier = () => world.player;

        const itemDeserializer = new ItemDeserializer(world.itemTypes);
        const changeHandlers = new ChangeHandlers()
            .withChangeHandler(new LootChestChangeHandler(world))
            .withChangeHandler(new PlayerChangeHandler(players, new PlayerDeserializer()))
            .withChangeHandler(new InventoryChangeHandler(
                world.playerId, 
                new InventoryDeserializer(itemDeserializer), 
                this.#inventoryModal.inventoryChangeListener
            ))
            .withChangeHandler(new EquippedWeaponChangeHandler(
                world.playerId,
                itemDeserializer,
                this.#inventoryModal.weaponChangeListener
            ));
        const updateHandler = new WorldUpdateHandler(world, changeHandlers)
            .withDynamicObjectDeserializer(new CharacterJsonDeserializer())
            .withDynamicObjectDeserializer(new ProjectileJsonDeserializer());

        // unregisters handleInit, switches to handling updates instead
        this.#handleMessage = (updateMessage) => updateHandler.handleWorldUpdate(updateMessage);
        
        setInterval(() => this.#canvas.draw(world), 1000 / 24);

        this.#world = world;
    }
}
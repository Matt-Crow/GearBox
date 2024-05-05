import { Canvas } from "./components/canvas.js";
import { GameOverScreen } from "./components/gameOverScreen.js";
import { InventoryModal } from "./components/inventoryModal.js";
import { PlayerHud } from "./components/playerHud.js";
import { CharacterJsonDeserializer } from "./model/character.js";
import { ItemDeserializer } from "./model/item.js";
import { LootChestJsonDeserializer } from "./model/lootChest.js";
import { PlayerChangeHandler } from "./model/player.js";
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

    #gameOverScreen;

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
     * @param {GameOverScreen} gameOverScreen 
     */
    constructor(canvas, inventoryModal, playerHud, gameOverScreen) {
        this.#canvas = canvas
        this.#inventoryModal = inventoryModal;
        this.#playerHud = playerHud;
        this.#gameOverScreen = gameOverScreen;
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

        const itemDeserializer = new ItemDeserializer(world.itemTypes);
        const updateHandler = new WorldUpdateHandler(world, itemDeserializer)
            .addGameObjectType(new CharacterJsonDeserializer())
            .addGameObjectType(new PlayerChangeHandler(world.playerId, this.#playerHud.playerUpdateListener))
            .addGameObjectType(new ProjectileJsonDeserializer())
            .addGameObjectType(new LootChestJsonDeserializer(world.playerId))
            .addUpdateListener(w => this.#gameOverScreen.update(w))
            .addInventoryChangeListener(inv => this.#inventoryModal.setInventory(inv))
            .addWeaponChangeListener(wea => this.#inventoryModal.setWeapon(wea))
            ;

        // unregisters handleInit, switches to handling updates instead
        this.#handleMessage = (updateMessage) => updateHandler.handleWorldUpdate(updateMessage);
        
        setInterval(() => this.#canvas.draw(world), 1000 / 24);

        this.#world = world;
    }
}
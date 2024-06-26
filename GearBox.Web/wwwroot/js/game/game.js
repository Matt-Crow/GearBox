import { Canvas } from "./components/canvas.js";
import { GameOverScreen } from "./components/gameOverScreen.js";
import { InventoryModal } from "./components/inventoryModal.js";
import { PlayerHud } from "./components/playerHud.js";
import { CharacterJsonDeserializer } from "./model/character.js";
import { ItemDeserializer } from "./model/item.js";
import { LootChestJsonDeserializer } from "./model/lootChest.js";
import { PlayerChangeHandler } from "./model/player.js";
import { ProjectileJsonDeserializer } from "./model/projectile.js";
import { WorldInitHandler, AreaUpdateHandler } from "./model/world.js";

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

    #areaUpdateHandler = () => {throw new Error("Cannot handle area update until after area init")};

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
        this.#world = null;
    }

    getPlayerCoords() {
        const player = this.#world?.player;
        return [player?.x, player?.y];
    }

    handleAreaInit(json) {
        const world = new WorldInitHandler()
            .handleWorldInit(json);
        this.#inventoryModal.setCraftingRecipes(world.craftingRecipes.recipes);

        const itemDeserializer = new ItemDeserializer(world.itemTypes);
        const updateHandler = new AreaUpdateHandler(world, itemDeserializer)
            .addGameObjectType(new CharacterJsonDeserializer())
            .addGameObjectType(new PlayerChangeHandler(world.playerId, this.#playerHud.playerUpdateListener))
            .addGameObjectType(new ProjectileJsonDeserializer())
            .addGameObjectType(new LootChestJsonDeserializer(world.playerId))
            .addUpdateListener(w => this.#gameOverScreen.update(w))
            .addInventoryChangeListener(inv => this.#inventoryModal.setInventory(inv))
            .addWeaponChangeListener(wea => this.#inventoryModal.setWeapon(wea))
            .addArmorChangeListener(arm => this.#inventoryModal.setArmor(arm))
            .addStatSummaryChangeListener(summary => this.#inventoryModal.setStatSummary(summary))
            ;

        this.#areaUpdateHandler = json => updateHandler.handleAreaUpdate(json);
        
        setInterval(() => this.#canvas.draw(world), 1000 / 24);

        this.#world = world;
    }

    handleAreaUpdate(json) {
        this.#areaUpdateHandler(json);
    }
}
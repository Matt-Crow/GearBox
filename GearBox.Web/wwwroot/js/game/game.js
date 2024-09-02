import { Canvas } from "./components/canvas.js";
import { GameOverScreen } from "./components/gameOverScreen.js";
import { InventoryModal } from "./components/inventoryModal.js";
import { PlayerHud } from "./components/playerHud.js";
import { handleGameInit } from "./messageHandlers/gameInitHandler.js";
import { characterDeserializer } from "./model/character.js";
import { InventoryDeserializer, ItemDeserializer } from "./model/item.js";
import { LootChestJsonDeserializer } from "./model/lootChest.js";
import { PlayerChangeHandler } from "./model/player.js";
import { projectileDeserializer } from "./model/projectile.js";
import { Area, AreaUpdateHandler } from "./model/area.js";
import { TileMap } from "./model/map.js";
import { ShopInitDeserializer } from "./model/shop.js";

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

    #gameData = null;

    #area; // required for getting mouse cursor position relative to player :(

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
        this.#area = null;
        setInterval(() => this.#draw(), 1000 / 24);
    }

    getPlayerCoords() {
        const player = this.#area?.player;
        return [player?.x, player?.y];
    }

    handleGameInit(json) {
        this.#gameData = handleGameInit(json);
        this.#inventoryModal.setCraftingRecipes(this.#gameData.craftingRecipes);
    }

    // todo rework this
    #handleAreaInit(json) {
        if (!this.#gameData) {
            throw new Error("Cannot accept area init until after game init");
        }

        const itemDeserializer = new ItemDeserializer(this.#gameData.itemTypes);
        const shopInitDeserializer = new ShopInitDeserializer(new InventoryDeserializer(itemDeserializer));
        const area = new Area(
            this.#gameData, 
            TileMap.fromJson(json.changes.map.value),
            json.changes.shops.value.map(j => shopInitDeserializer.deserialize(j))
        );
        const updateHandler = new AreaUpdateHandler(area, itemDeserializer)
            .addGameObjectType(characterDeserializer)
            .addGameObjectType(new PlayerChangeHandler(this.#gameData.playerId, this.#playerHud.playerUpdateListener))
            .addGameObjectType(projectileDeserializer)
            .addGameObjectType(new LootChestJsonDeserializer(this.#gameData.playerId))
            .addUpdateListener(w => this.#gameOverScreen.update(w))
            .addInventoryChangeListener(inv => this.#inventoryModal.setInventory(inv))
            .addWeaponChangeListener(wea => this.#inventoryModal.setWeapon(wea))
            .addArmorChangeListener(arm => this.#inventoryModal.setArmor(arm))
            .addStatSummaryChangeListener(summary => this.#inventoryModal.setStatSummary(summary))
            ;

        this.#areaUpdateHandler = json => updateHandler.handleAreaUpdate(json);
        
        this.#area = area;
    }

    handleAreaUpdate(json) {
        if (json.changes.map.hasChanged || json.changes.shops.hasChanged) {
            this.#handleAreaInit(json);
        }
        this.#areaUpdateHandler(json);
    }

    #draw() {
        if (this.#area) {
            this.#canvas.draw(this.#area);
        }
    }
}
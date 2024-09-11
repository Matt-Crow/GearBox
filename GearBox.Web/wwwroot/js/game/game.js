import { Canvas } from "./components/canvas.js";
import { GameOverScreen } from "./components/gameOverScreen.js";
import { PlayerHud } from "./components/playerHud.js";
import { handleGameInit } from "./messageHandlers/gameInitHandler.js";
import { characterDeserializer } from "./model/character.js";
import { ItemDeserializer } from "./model/item.js";
import { LootChestJsonDeserializer } from "./model/lootChest.js";
import { PlayerChangeHandler } from "./model/player.js";
import { projectileDeserializer } from "./model/projectile.js";
import { Area, AreaUpdateHandler } from "./model/area.js";
import { TileMap } from "./model/map.js";
import { Shop } from "./model/shop.js";
import { MainModal } from "./components/mainModal.js";

export class Game {

    /**
     * The custom canvas component this draws on.
     */
    #canvas;

    #mainModal;

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
     * @param {MainModal} mainModal 
     * @param {PlayerHud} playerHud the player HUD for the current player
     * @param {GameOverScreen} gameOverScreen 
     */
    constructor(canvas, mainModal, playerHud, gameOverScreen) {
        this.#canvas = canvas
        this.#mainModal = mainModal;
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
        this.#mainModal.setCraftingRecipes(this.#gameData.craftingRecipes);
    }

    // todo rework this
    #handleAreaInit(json) {
        if (!this.#gameData) {
            throw new Error("Cannot accept area init until after game init");
        }

        const area = new Area(
            this.#gameData, 
            TileMap.fromJson(json.changes.map.value),
            json.changes.shops.value.map(j => Shop.fromJson(j))
        );
        const itemDeserializer = new ItemDeserializer(this.#gameData.itemTypes);
        const updateHandler = new AreaUpdateHandler(area, itemDeserializer)
            .addGameObjectType(characterDeserializer)
            .addGameObjectType(new PlayerChangeHandler(this.#gameData.playerId, this.#playerHud.playerUpdateListener))
            .addGameObjectType(projectileDeserializer)
            .addGameObjectType(new LootChestJsonDeserializer(this.#gameData.playerId))
            .addUpdateListener(w => this.#gameOverScreen.update(w))
            .addInventoryChangeListener(inv => this.#mainModal.setInventory(inv))
            .addWeaponChangeListener(wea => this.#mainModal.setWeapon(wea))
            .addArmorChangeListener(arm => this.#mainModal.setArmor(arm))
            .addStatSummaryChangeListener(summary => this.#mainModal.setStatSummary(summary))
            .addOpenShopChangeListener(openShop => this.#mainModal.setShop(openShop))
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
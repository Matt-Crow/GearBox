import { handleGameInit } from "./messageHandlers/gameInitHandler.js";
import { characterDeserializer } from "./model/character.js";
import { LootChestJsonDeserializer } from "./model/lootChest.js";
import { PlayerJsonDeserializer } from "./model/player.js";
import { projectileDeserializer } from "./model/projectile.js";
import { Area, AreaUpdateHandler } from "./model/area.js";
import { TileMap } from "./model/map.js";
import { Shop } from "./model/shop.js";
import { Views } from "./components/views/views.js";

export class Game {
    #views;

    #areaUpdateHandler = () => {throw new Error("Cannot handle area update until after area init")};

    #gameData = null;

    #area; // required for getting mouse cursor position relative to player :(

    /**
     * @param {Views} views 
     */
    constructor(views) {
        this.#views = views;
        this.#area = null;
        setInterval(() => this.#draw(), 1000 / 24);
    }

    getPlayerCoords() {
        const player = this.#area?.player;
        return [player?.x, player?.y];
    }

    handleGameInit(json) {
        this.#gameData = handleGameInit(json);
        this.#views.viewAlive.mainModal.setCraftingRecipes(this.#gameData.craftingRecipes);
    }

    // todo rework this
    #handleAreaInit(json) {
        if (!this.#gameData) {
            throw new Error("Cannot accept area init until after game init");
        }

        const area = new Area(
            this.#gameData, 
            TileMap.fromJson(json.uiStateChanges.area.value.map),
            json.uiStateChanges.area.value.shops.map(j => Shop.fromJson(j))
        );
        const updateHandler = new AreaUpdateHandler(area, this.#views)
            .addGameObjectType(characterDeserializer)
            .addGameObjectType(new PlayerJsonDeserializer())
            .addGameObjectType(projectileDeserializer)
            .addGameObjectType(new LootChestJsonDeserializer(this.#gameData.playerId))
            .addUpdateListener(w => this.#views.handleAreaUpdate(w))
            ;

        this.#areaUpdateHandler = json => updateHandler.handleAreaUpdate(json);
        
        this.#area = area;
    }

    handleAreaUpdate(json) {
        if (json.uiStateChanges.area.hasChanged) {
            this.#handleAreaInit(json);
        }
        this.#areaUpdateHandler(json);
    }

    #draw() {
        if (this.#area) {
            this.#views.viewAlive.canvas.draw(this.#area);
        }
    }
}
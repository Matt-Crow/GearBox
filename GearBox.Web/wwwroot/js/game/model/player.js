/*
    Exports:
    - Player: model class
    - PlayerChangeHandler: responds to player updates
    - PlayerRepository: stores players
    - PlayerEventListener: listens for events emitted by PlayerRepository
 */

import { ChangeHandler } from "../infrastructure/change.js";
import { TestCase, TestSuite } from "../testing/tests.js";
import { Inventory, InventoryDeserializer } from "./item.js";

export class Player {
    #id;
    #inventory;

    /**
     * @param {string} id 
     * @param {Inventory} inventory 
     */
    constructor(id, inventory) {
        this.#id = id;
        this.#inventory = inventory ?? new Inventory();
    }

    get id() {
        return this.#id;
    }

    get inventory() {
        return this.#inventory;
    }
}

export class PlayerDeserializer {
    #inventoryDeserializer;

    /**
     * @param {InventoryDeserializer} inventoryDeserializer 
     */
    constructor(inventoryDeserializer) {
        this.#inventoryDeserializer = inventoryDeserializer;
    }

    deserialize(json) {
        const id = json.id;
        const inventory = this.#inventoryDeserializer.deserialize(json.inventory);
        return new Player(id, inventory);
    }
}

export class PlayerChangeHandler extends ChangeHandler {
    #players;
    #deserializer;

    /** 
     * @param {PlayerRepository} players 
     * @param {PlayerDeserializer} deserializer 
     */    
    constructor(players, deserializer) {
        super("playerCharacter");
        this.#players = players;
        this.#deserializer = deserializer;
    }

    handleContent(obj) {
        const player = this.#deserializer.deserialize(obj);
        this.#players.savePlayer(player);
    }

    handleDelete(obj) {
        this.#players.removePlayerById(obj.id);
    }
}

export class PlayerRepository {
    #players = new Map();
    #playerListeners = new Map();

    /**
     * @param {Player} player 
     */
    savePlayer(player) {
        this.#players.set(player.id, player);
        this.#notifyListeners(player.id, (listener) => listener.playerChanged(player));
    }

    /**
     * @param {number} playerId
     * @param {PlayerEventListener} listener
     */
    addPlayerListener(playerId, listener) {
        if (!this.#playerListeners.has(playerId)) {
            this.#playerListeners.set(playerId, []);
        }
        this.#playerListeners.get(playerId).push(listener);
    }

    /**
     * @param {number} id
     * @returns {Player?}
     */
    getPlayerById(id) {
        const result = this.#players.has(id)
            ? this.#players.get(id)
            : null;
        return result;
    }

    /**
     * @param {number} id 
     */
    removePlayerById(id) {
        const player = this.getPlayerById(id);
        if (player === null) {
            throw new Error(`Player not found: ${id}`);
        }
        this.#players.delete(id);
        this.#notifyListeners(id, (listener) => listener.playerRemoved(player));
    }

    /**
     * @param {number} playerId
     * @param {(listener: PlayerEventListener) => any} doThis
     */
    #notifyListeners(playerId, doThis) {
        const listeners = this.#playerListeners.get(playerId);
        if (listeners) {
            listeners.forEach((listener) => doThis(listener));
        }
    }
}

export class PlayerEventListener {
    #onPlayerChanged;
    #onPlayerRemoved;

    constructor({onPlayerChanged, onPlayerRemoved}) {
        const doNothing = () => {};
        this.#onPlayerChanged = onPlayerChanged ?? doNothing;
        this.#onPlayerRemoved = onPlayerRemoved ?? doNothing;
    }

    playerChanged(player) {
        this.#onPlayerChanged(player);
    }

    playerRemoved(player) {
        this.#onPlayerRemoved(player);
    }
}

export const playerTests = new TestSuite("PlayerRepository", [
    new TestCase("savePlayer_givenDuplicate_isOk", (assert) => {
        const sut = new PlayerRepository();
        const player = new Player(42);

        sut.savePlayer(player);
        sut.savePlayer(player);
    }),
    new TestCase("savePlayer_givenPlayer_works", (assert) => {
        const sut = new PlayerRepository();
        const expected = new Player(42);

        sut.savePlayer(expected);
        const actual = sut.getPlayerById(expected.id);

        assert.equal(expected, actual);
    }),
    new TestCase("savePlayer_notifiesChangeListeners", (assert) => {
        const sut = new PlayerRepository();
        let called = false;
        const listener = new PlayerEventListener({
            onPlayerChanged: () => called = true
        });
        sut.addPlayerListener(42, listener);

        sut.savePlayer(new Player(42));

        assert.assert(called, "playerAdded should have been called");
    }),
    new TestCase("savePlayer_alsoUpdates", (assert) => {
        const old = new Player(42);
        const expected = new Player(42);
        const sut = new PlayerRepository();
        sut.savePlayer(old);

        sut.savePlayer(expected);
        const actual = sut.getPlayerById(42);

        assert.notEqual(old, actual);
        assert.equal(expected, actual);
    }),
    new TestCase("getPlayerById_givenNotFound_returnsNull", (assert) => {
        const sut = new PlayerRepository();
        const actual = sut.getPlayerById(42);
        assert.isNull(actual);
    }),
    new TestCase("remove_givenBadId_throws", (assert) => {
        const sut = new PlayerRepository();

        assert.throws(() => sut.removePlayerById(42));
    }),
    new TestCase("remove_works", (assert) => {
        const sut = new PlayerRepository();
        sut.savePlayer(new Player(42));

        sut.removePlayerById(42);
        assert.isNull(sut.getPlayerById(42));
    }),
    new TestCase("remove_notifiesChangeListeners", (assert) => {
        const sut = new PlayerRepository();
        const player = new Player(42);
        sut.savePlayer(player);
        let called = false;
        const listener = new PlayerEventListener({
            onPlayerRemoved: () => called = true
        });
        sut.addPlayerListener(player.id, listener);

        sut.removePlayerById(player.id);

        assert.assert(called, "playerRemoved should have been called");
    })
]);
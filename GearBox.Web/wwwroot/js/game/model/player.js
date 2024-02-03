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

    /*
     * @param {PlayerRepository} players 
     * @param {PlayerDeserializer} deserializer 
     */    
    constructor(players, deserializer) {
        super("playerCharacter");
        this.#players = players;
        this.#deserializer = deserializer;
    }

    handleCreate(body) {
        console.log(`Create player ${body}`);
        const json = JSON.parse(body);
        const player = this.#deserializer.deserialize(json);
        this.#players.addPlayer(player);
    }

    handleUpdate(body) {
        console.log(`Update player ${body}`);
        const json = JSON.parse(body);
        const player = this.#deserializer.deserialize(json);
        this.#players.updatePlayer(player);
    }

    handleDelete(body) {
        console.log(`Delete player ${body}`);
        this.#players.removePlayerById(JSON.parse(body).id);
    }
}

export class PlayerRepository {
    #players = new Map();
    #playerListeners = new Map();

    /**
     * @param {Player} player 
     */
    addPlayer(player) {
        if (this.getPlayerById(player.id)) {
            throw new Error(`Player already exists with id ${player.id}`);
        }
        this.#players.set(player.id, player);
        this.#notifyListeners(player.id, (listener) => listener.playerCreated(player));
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
     * @param {Player} player 
     */
    updatePlayer(player) {
        if (!this.getPlayerById(player.id)) {
            throw new Error(`Player not found: ${player.id}`);
        }
        this.#players.set(player.id, player);
        this.#notifyListeners(player.id, (listener) => listener.playerUpdated(player));
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
    #onPlayerAdded;
    #onPlayerUpdated;
    #onPlayerRemoved;

    constructor({onPlayerAdded, onPlayerUpdated, onPlayerRemoved}) {
        const doNothing = () => {};
        this.#onPlayerAdded = onPlayerAdded ?? doNothing;
        this.#onPlayerUpdated = onPlayerUpdated ?? doNothing;
        this.#onPlayerRemoved = onPlayerRemoved ?? doNothing;
    }

    playerCreated(player) {
        this.#onPlayerAdded(player);
    }

    playerUpdated(player) {
        this.#onPlayerUpdated(player);
    }

    playerRemoved(player) {
        this.#onPlayerRemoved(player);
    }
}

export const playerTests = new TestSuite("PlayerRepository", [
    new TestCase("add_givenDuplicate_throws", (assert) => {
        const sut = new PlayerRepository();
        const player = new Player(42);

        sut.addPlayer(player);
        assert.throws(() => sut.addPlayer(player));
    }),
    new TestCase("add_givenPlayer_works", (assert) => {
        const sut = new PlayerRepository();
        const expected = new Player(42);

        sut.addPlayer(expected);
        const actual = sut.getPlayerById(expected.id);

        assert.equal(expected, actual);
    }),
    new TestCase("add_notifiesChangeListeners", (assert) => {
        const sut = new PlayerRepository();
        let called = false;
        const listener = new PlayerEventListener({
            onPlayerAdded: () => called = true
        });
        sut.addPlayerListener(42, listener);

        sut.addPlayer(new Player(42));

        assert.assert(called, "playerAdded should have been called");
    }),
    new TestCase("getPlayerById_givenNotFound_returnsNull", (assert) => {
        const sut = new PlayerRepository();
        const actual = sut.getPlayerById(42);
        assert.isNull(actual);
    }),
    new TestCase("update_givenNewPlayer_throws", (assert) => {
        const sut = new PlayerRepository();

        assert.throws(() => sut.updatePlayer(new Player(42)));
    }),
    new TestCase("update_works", (assert) => {
        const old = new Player(42);
        const expected = new Player(42);
        const sut = new PlayerRepository();
        sut.addPlayer(old);

        sut.updatePlayer(expected);
        const actual = sut.getPlayerById(42);

        assert.notEqual(old, actual);
        assert.equal(expected, actual);
    }),
    new TestCase("update_notifiesChangeListeners", (assert) => {
        const sut = new PlayerRepository();
        const player = new Player(42);
        sut.addPlayer(player);
        let called = false;
        const listener = new PlayerEventListener({
            onPlayerUpdated: () => called = true
        });
        sut.addPlayerListener(player.id, listener);

        sut.updatePlayer(player);

        assert.assert(called, "playerUpdated should have been called");
    }),
    new TestCase("remove_givenBadId_throws", (assert) => {
        const sut = new PlayerRepository();

        assert.throws(() => sut.removePlayerById(42));
    }),
    new TestCase("remove_works", (assert) => {
        const sut = new PlayerRepository();
        sut.addPlayer(new Player(42));

        sut.removePlayerById(42);
        assert.isNull(sut.getPlayerById(42));
    }),
    new TestCase("remove_notifiesChangeListeners", (assert) => {
        const sut = new PlayerRepository();
        const player = new Player(42);
        sut.addPlayer(player);
        let called = false;
        const listener = new PlayerEventListener({
            onPlayerRemoved: () => called = true
        });
        sut.addPlayerListener(player.id, listener);

        sut.removePlayerById(player.id);

        assert.assert(called, "playerRemoved should have been called");
    })
]);
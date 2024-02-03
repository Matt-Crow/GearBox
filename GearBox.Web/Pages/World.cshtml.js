import { Game } from "../js/game/game.js";
import { Client } from "../js/game/infrastructure/client.js";

$(async () => await main());

async function main() {
    const canvas = document.querySelector("#canvas"); // don't want some weird JQuery proxy
    if (canvas === null) {
        throw new Error("Canvas not found.");
    }

    const inventoryModal = document.querySelector("#inventoryModal");
    if (inventoryModal === null) {
        throw new Error("Inventory modal not found.");
    }

    const inventoryRows = document.querySelector("#inventoryRows");
    if (inventoryRows === null) {
        throw new Error("Inventory rows not found.");
    }

    const game = new Game(canvas, inventoryRows);

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/world-hub")
        .build();
    connection.on("receive", (message) => {
        const obj = JSON.parse(message);
        try {
            game.handle(obj);
        } catch (e) {
            // set breakpoint here if SignalR keeps suppressing error messages
            console.error(e);
        }
    });
    await connection.start();

    const client = new Client(connection);
    const keyMappings = new Map(); // value is [onUp, onDown]
    keyMappings.set("KeyW", [() => client.stopMovingUp(),    () => client.startMovingUp()]);
    keyMappings.set("KeyA", [() => client.stopMovingLeft(),  () => client.startMovingLeft()]);
    keyMappings.set("KeyS", [() => client.stopMovingDown(),  () => client.startMovingDown()]);
    keyMappings.set("KeyD", [() => client.stopMovingRight(), () => client.startMovingRight()]);
    document.addEventListener("keyup", e => {
        // uses e.repeat to check if key is held down
        if (keyMappings.has(e.code) && !e.repeat) {
            keyMappings.get(e.code)[0]();
        }
    });
    document.addEventListener("keydown", e => {
        // uses e.repeat to check if key is held down
        if (keyMappings.has(e.code) && !e.repeat) {
            keyMappings.get(e.code)[1]();
        }
        if (e.code == "KeyI") {
            if (inventoryModal.open) {
                inventoryModal.close();
            } else {
                inventoryModal.showModal();
            }
        }
    });
}

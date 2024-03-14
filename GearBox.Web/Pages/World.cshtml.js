import { Game } from "../js/game/game.js";
import { InventoryModal } from "../js/game/components/inventory.js";
import { Client } from "../js/game/infrastructure/client.js";

$(async () => await main());

async function main() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/world-hub")
        .build();
    const client = new Client(connection);
    const canvas = findElement("#canvas"); 
    const inventoryModal = new InventoryModal(
        findElement("#inventoryModal"), 
        findElement("#materialRows"),
        findElement("#equipmentRows"),
        client
    );
    const playerHud = document.getElementById("playerHud");
    const game = new Game(canvas, inventoryModal, playerHud);
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
            inventoryModal.toggle();
        }
    });
}

function findElement(selector) {
    const e = document.querySelector(selector);
    if (e === null) {
        throw new Error(`Failed to locate element "${selector}"`);
    }
    return e;
}

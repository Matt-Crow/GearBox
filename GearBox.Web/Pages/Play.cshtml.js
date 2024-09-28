import { Game } from "../js/game/game.js";
import { Client } from "../js/game/infrastructure/client.js";
import { MainModal } from "../js/game/components/mainModal.js";
import { Views } from "../js/game/components/views/views.js";

$(async () => await main());

async function main() {
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/area-hub")
    .build();
    const client = new Client(connection);
    $("#respawn-button").on("click", () => client.respawn());
    
    const views = new Views(); // todo move more stuff into this
    const mainModal = new MainModal("#main-modal", client);
    const canvas = views.viewAlive.canvas;
    const game = new Game(mainModal, views);
    
    connection.on("GameInit", handle(json => game.handleGameInit(json)));
    connection.on("AreaUpdate", handle(json => game.handleAreaUpdate(json)));
    
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
        if (e.code == "KeyQ") {
            const [px, py] = game.getPlayerCoords();
            const dx = canvas.translatedMouseX - px;
            const dy = canvas.translatedMouseY - py;
            const angleInRadians = Math.atan2(-dy, dx); // y first, then x. -dy flips
            const angleInDegrees = Math.trunc(180 * angleInRadians / Math.PI); // convert to int so it doesn't crash server
            const bearing = 90 - angleInDegrees;
            client.useBasicAttack(bearing);
        }
        if (e.code == "KeyE") { 
            mainModal.toggle();
            client.openShop();
        }
    });

    findElement("#main-modal .shop").addEventListener("close", () => client.closeShop());
}

function handle(handler) {
    const pipeline = new Pipeline()
        .then(aString => JSON.parse(aString))
        .then(json => handler(json));
    return (message) => pipeline.consume(message);
}

function findElement(selector) {
    const e = document.querySelector(selector);
    if (e === null) {
        throw new Error(`Failed to locate element "${selector}"`);
    }
    return e;
}

class Pipeline {
    #transforms = [];

    then(transform) {
        this.#transforms.push(transform);
        return this;
    }

    consume(obj) {
        let currValue = obj;
        try {
            for (let transform of this.#transforms) {
                currValue = transform(currValue);
            }
        } catch(e) {
            // set breakpoint here if SignalR keeps suppressing error messages
            console.error(e);
        }
    }
}

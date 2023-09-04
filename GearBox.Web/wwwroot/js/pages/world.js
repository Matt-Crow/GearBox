import { Game } from "../game/game.js";

$(async () => await main());

async function main() {
    const canvas = document.getElementById("canvas"); // don't want some weird JQuery proxy
    if (canvas === null) {
        throw new Error("Canvas not found.");
    }

    const game = new Game(canvas);

    $("#submit").prop("disabled", true);
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

    $("#submit").on("click", (e) => {
        const message = $("input").val();
        connection.invoke("Send", message);
        e.preventDefault();
    });
    $("#submit").prop("disabled", false);
}
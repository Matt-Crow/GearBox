import { Game } from "../game/game.js";

$(async () => await main());

async function main() {
    const game = new Game();

    $("#submit").prop("disabled", true);
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/world-hub")
        .build();
    connection.on("receive", (message) => {
        const obj = JSON.parse(message);
        game.handle(obj);
    });
    await connection.start();

    $("#submit").on("click", (e) => {
        const message = $("input").val();
        connection.invoke("Send", message);
        e.preventDefault();
    });
    $("#submit").prop("disabled", false);
}
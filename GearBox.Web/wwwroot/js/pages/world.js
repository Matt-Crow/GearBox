$(async () => await main());

async function main() {
    $("#submit").prop("disabled", true);
    
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/world-hub")
        .build();
    connection.on("receive", (message) => {
        const li = $("<li></li>");
        li.text(message);
        li.appendTo("#output");
    });
    await connection.start();

    $("#submit").on("click", (e) => {
        const message = $("input").val();
        connection.invoke("Send", message);
        e.preventDefault();
    });
    $("#submit").prop("disabled", false);
}
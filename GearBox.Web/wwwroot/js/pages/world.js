$(async () => await main());

async function main() {
    $("#submit").prop("disabled", true);
    
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/world-hub")
        .build();
    connection.on("receive", (message) => {
        const obj = JSON.parse(message);
        const pretty = JSON.stringify(obj, null, 4);
        $("#output").text(pretty);
    });
    await connection.start();

    $("#submit").on("click", (e) => {
        const message = $("input").val();
        connection.invoke("Send", message);
        e.preventDefault();
    });
    $("#submit").prop("disabled", false);
}
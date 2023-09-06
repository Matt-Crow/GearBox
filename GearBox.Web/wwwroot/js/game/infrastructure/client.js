export class Client {
    #signalr;

    constructor(signalr) {
        this.#signalr = signalr;    
    }

    startMovingUp() {
        this.#signalr.invoke("StartMovingUp");
    }

    startMovingDown() {
        this.#signalr.invoke("StartMovingDown");
    }

    startMovingLeft() {
        this.#signalr.invoke("StartMovingLeft");
    }

    startMovingRight() {
        this.#signalr.invoke("StartMovingRight");
    }

    stopMovingUp() {
        this.#signalr.invoke("StopMovingUp");
    }

    stopMovingDown() {
        this.#signalr.invoke("StopMovingDown");
    }

    stopMovingLeft() {
        this.#signalr.invoke("StopMovingLeft");
    }

    stopMovingRight() {
        this.#signalr.invoke("StopMovingRight");
    }
}
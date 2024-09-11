export class Client {
    #signalr;

    constructor(signalr) {
        this.#signalr = signalr;    
    }

    equipWeapon(id) {
        this.#signalr.invoke("EquipWeapon", id)
    }

    equipArmor(id) {
        this.#signalr.invoke("EquipArmor", id)
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

    craft(recipeId) {
        this.#signalr.invoke("Craft", recipeId);
    }

    openShop() {
        this.#signalr.invoke("OpenShop");
    }

    closeShop() {
        this.#signalr.invoke("CloseShop");
    }

    shopBuy(shopId, itemId, itemName) {
        this.#signalr.invoke("ShopBuy", shopId, itemId, itemName);
    }

    shopSell(shopId, itemId, itemName) {
        this.#signalr.invoke("ShopSell", shopId, itemId, itemName);
    }

    useBasicAttack(bearingInDegrees) {
        this.#signalr.invoke("UseBasicAttack", bearingInDegrees);
    }

    respawn() {
        this.#signalr.invoke("Respawn");
    }
}
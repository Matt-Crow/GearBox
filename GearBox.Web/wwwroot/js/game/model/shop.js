import { getColorStringFromJson } from "./color.js";
import { PIXELS_PER_TILE } from "./constants.js";
import { Item } from "./item.js";

export class Shop {
    #name;
    #xInPixels;
    #yInPixels;
    #color;

    /**
     * 
     * @param {string} name 
     * @param {number} xInPixels 
     * @param {number} yInPixels 
     * @param {string} color 
     */
    constructor(name, xInPixels, yInPixels, color) {
        this.#name = name;
        this.#xInPixels = xInPixels;
        this.#yInPixels = yInPixels;
        this.#color = color;
    }

    /**
     * @param {CanvasRenderingContext2D} context the canvas to draw on
     */
    draw(context) {
        context.fillStyle = this.#color;
        context.fillRect(this.#xInPixels, this.#yInPixels, PIXELS_PER_TILE, PIXELS_PER_TILE);

        context.fillStyle = "black";
        context.fillText(this.#name, this.#xInPixels, this.#yInPixels - PIXELS_PER_TILE / 4);
    }

    static fromJson(json) {
        const result = new Shop(
            json.name,
            json.xInPixels,
            json.yInPixels,
            getColorStringFromJson(json.color)
        );
        return result;
    }
}

export class OpenShop {
    #id;
    #name;
    #playerGold;
    #buyOptions;
    #sellOptions;
    #buybackOptions;

    constructor(id, name, playerGold, buyOptions, sellOptions, buybackOptions) {
        this.#id = id;
        this.#name = name;
        this.#playerGold = playerGold;
        this.#buyOptions = buyOptions;
        this.#sellOptions = sellOptions;
        this.#buybackOptions = buybackOptions;
    }

    get id() { return this.#id; }
    get name() { return this.#name; }
    get playerGold() { return this.#playerGold; }
    get buyOptions() { return this.#buyOptions; }
    get sellOptions() { return this.#sellOptions; }
    get buybackOptions() { return this.#buybackOptions; }
}

export class OpenShopOption {
    #item;
    #price;
    #canAfford;

    /**
     * @param {Item} item 
     * @param {number} price 
     * @param {boolean} canAfford 
     */
    constructor(item, price, canAfford) {
        this.#item = item;
        this.#price = price;
        this.#canAfford = canAfford;
    }

    get item() { return this.#item; }
    get price() { return this.#price; }
    get canAfford() { return this.#canAfford; }
}

export class OpenShopDeserializer {
    deserialize(json) {
        if (!json) {
            return null;
        }
        const result = new OpenShop(
            json.id,
            json.name,
            json.playerGold,
            this.#deserializeOptions(json.buyOptions),
            this.#deserializeOptions(json.sellOptions),
            this.#deserializeOptions(json.buybackOptions)
        );
        return result;
    }

    #deserializeOptions(json) {
        return json.map(option => this.#deserializeOption(option));
    }

    #deserializeOption(option) {
        const item = Item.fromJson(option.item);
        return new OpenShopOption(item, option.buyPrice, option.canAfford);
    }
}
import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";

export class Character {
    #xInPixels;
    #yInPixels;

    constructor({xInPixels, yInPixels}) {
        this.#xInPixels = xInPixels;
        this.#yInPixels = yInPixels;
    }
}

export class CharacterJsonDeserializer extends JsonDeserializer {
    constructor() {
        super("character", (obj) => new Character(obj));
    }
}
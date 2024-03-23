import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { PIXELS_PER_TILE } from "./constants.js";

export class Projectile {
    #x;
    #y;

    constructor(x, y) {
        this.#x = x;
        this.#y = y;
    }

    get x() { return this.#x; }
    get y() { return this.#y; }

    /**
     * @param {CanvasRenderingContext2D} context 
     */
    draw(context) {
        const r = PIXELS_PER_TILE / 2;
        context.fillStyle = "red";

        // https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/arc
        context.beginPath();
        context.arc(this.#x, this.#y, r, 0, 2*Math.PI);
        context.fill();
    }
}

export class ProjectileJsonDeserializer extends JsonDeserializer {
    constructor() {
        super("projectile", obj => new Projectile(obj.x, obj.y));
    }
}
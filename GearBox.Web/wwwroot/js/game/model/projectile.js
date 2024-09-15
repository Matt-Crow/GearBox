import { JsonDeserializer } from "../infrastructure/jsonDeserializer.js";
import { getColorStringFromJson } from "./color.js";

export class Projectile {
    #x;
    #y;
    #radius;
    #bearingInDegrees;
    #color;

    constructor(x, y, radius, bearingInDegrees, color) {
        this.#x = x;
        this.#y = y;
        this.#radius = radius;
        this.#bearingInDegrees = bearingInDegrees;
        this.#color = color;
    }

    get x() { return this.#x; }
    get y() { return this.#y; }

    /**
     * @param {CanvasRenderingContext2D} context 
     */
    draw(context) {
        const r = this.#radius;
        context.fillStyle = this.#color;

        // draw a triangle
        const theta = (90 - this.#bearingInDegrees)*Math.PI / 180;
        const dt = 2*Math.PI / 3;
        context.beginPath();
        for (let i = 0; i < 3; i++) {
            context.lineTo(
                this.#x + r*Math.cos(theta+i*dt),
                this.#y - r*Math.sin(theta+i*dt)
            );
        }
        context.fill();
    }
}

export const projectileDeserializer = new JsonDeserializer("projectile", json => new Projectile(
    json.x, 
    json.y, 
    json.radius, 
    json.bearingInDegrees, 
    getColorStringFromJson(json.color)
));
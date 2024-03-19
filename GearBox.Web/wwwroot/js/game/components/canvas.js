import { World } from "../model/world.js";

export class Canvas {
    #element;
    #mouseX = 0;
    #mouseY = 0;
    #translateX = 0;
    #translateY = 0;

    /**
     * @param {HTMLCanvasElement} element 
     */
    constructor(element) {
        this.#element = element;
        element.addEventListener("mousemove", e => this.#mouseMoved(e));
    }

    /**
     * the X-coordinate of the mouse cursor on the world this is drawing
     */
    get translatedMouseX() { return this.#mouseX - this.#translateX; }

    /**
     * the Y-coordinate of the mouse cursor on the world this is drawing
     */
    get translatedMouseY() { return this.#mouseY - this.#translateY; }

    /**
     * https://stackoverflow.com/a/17130415
     * @param {MouseEvent} e 
     */
    #mouseMoved(e) {
        const box = this.#element.getBoundingClientRect();
        this.#mouseX = e.clientX - box.left;
        this.#mouseY = e.clientY - box.top;
    }

    /**
     * @param {number} x 
     * @param {number} y 
     */
    translate(x, y) {
        this.#translateX = x;
        this.#translateY = y;
    }

    /**
     * @param {World} world 
     */
    draw(world) {
        const player = world.player;
        const w = this.#element.width;
        const h = this.#element.height;
        if (player) {
            // don't translate if player is dead
            // that way, the camera doesn't reset to [0, 0]
            this.translate(
                clamp(w - world.widthInPixels, w/2 - player.x, 0), 
                clamp(h - world.heightInPixels, h/2 - player.y, 0)
            );
        }
        const ctx = this.#element.getContext("2d");
        ctx.clearRect(0, 0, w, h);
        ctx.translate(this.#translateX, this.#translateY);
        world.draw(ctx);
        ctx.resetTransform();
    }
}

function clamp(min, x, max) {
    if (x > max) {
        return max;
    }
    if (x < min) {
        return min;
    }
    return x;
}
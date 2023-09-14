import { ChangeHandler } from "../infrastructure/change.js";

export class PlayerChangeHandler extends ChangeHandler {
    constructor() {
        super("playerCharacter");
    }

    handleCreate(body) {
        console.log(`Create player ${body}`);
    }

    handleUpdate(body) {
        console.log(`Update player ${body}`);
    }

    handleDelete(body) {
        console.log(`Delete player ${body}`);
    }
}
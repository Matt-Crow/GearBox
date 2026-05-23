export class PassiveAbility {
    #name;
    #description;

    constructor(name, description) {
        this.#name = name;
        this.#description = description;
    }

    get name() { return this.#name; }
    get description() { return this.#description; }

    static fromJson(json) {
        const result = new PassiveAbility(
            json.name,
            json.description
        );
        return result;
    }
}
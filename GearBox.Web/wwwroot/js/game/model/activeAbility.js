export class ActiveAbility {
    #name;
    #energyCost;
    #secondsUntilNextUse;
    #canBeUsed;
    #description;

    constructor(name, energyCost, secondsUntilNextUse, canBeUsed, description) {
        this.#name = name;
        this.#energyCost = energyCost;
        this.#secondsUntilNextUse = secondsUntilNextUse;
        this.#canBeUsed = canBeUsed;
        this.#description = description;
    }

    get name() { return this.#name; }
    get energyCost() { return this.#energyCost; }
    get secondsUntilNextUse() { return this.#secondsUntilNextUse; }
    get canBeUsed() { return this.#canBeUsed; }
    get description() { return this.#description; }

    static fromJson(json) {
        const result = new ActiveAbility(
            json.name,
            json.energyCost,
            json.secondsUntilNextUse,
            json.canBeUsed,
            json.description
        );
        return result;
    }
}
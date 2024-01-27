export class Assertions {
    assert(someBool, errorMessage="assertion failed") {
        if (!someBool) {
            throw new Error(errorMessage);
        }
    }
}
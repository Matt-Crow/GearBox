export class Assertions {
    assert(someBool, errorMessage="assertion failed") {
        if (!someBool) {
            throw new Error(errorMessage);
        }
    }

    equal(a, b, comparator=defaultComparator) {
        this.assert(comparator(a, b), "should be equal");
    }

    notEqual(a, b, comparator=defaultComparator) {
        this.assert(!comparator(a, b), "should not be equal");
    }

    isNull(someObject) {
        this.assert(someObject === null, "should be null");
    }

    /**
     * @param {() => any} someFunction 
     */
    throws(someFunction) {
        let threwAnError = false;
        try {
            someFunction();
        } catch(ex) {
            threwAnError = true;
        }
        this.assert(threwAnError, "should have thrown an error");
    }
}

function defaultComparator(a, b) {
    return a === b;
}
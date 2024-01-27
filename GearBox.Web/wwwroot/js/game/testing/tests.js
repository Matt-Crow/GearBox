/**
 * this module contains the core test classes, but no implementations
 */
import {Assertions} from './assertions.js';

export class TestSuite {
    #name;
    #tests;

    /**
     * @param {string} name the name to display for this test suite
     * @param {TestCase[]} tests the tests contained in this suite
     */
    constructor(name, tests) {
        this.#name = name;
        this.#tests = tests.slice(); // shallow copy
    }

    get name() {
        return this.#name;
    }

    get tests() {
        return this.#tests.slice();
    }
}

export class TestCase {
    #name;
    #run;

    /**
     * @param {string} name the name to display for this test case
     * @param {(assertions: Assertions) => any} run the test function to run. If it throws an exception, the test fails. The result will be awaited.
     */
    constructor(name, run) {
        this.#name = name;
        this.#run = run;
    }

    get name() {
        return this.#name;
    }

    /**
     * @param {Assertions} assertions 
     */
    async run(assertions) {
        await this.#run(assertions);
    }
}

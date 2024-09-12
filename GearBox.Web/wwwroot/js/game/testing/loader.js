/**
 * use this to export all the "real" tests used by the program
 */
import { TestCase, TestSuite } from "./tests.js";

/**
 * @returns TestSuite[]
 */
export function getAllTestSuites() {
    const allTestSuites = [
        new TestSuite("test suite 1", [
            new TestCase("async test", async () => {
                // https://stackoverflow.com/a/39914235
                await new Promise(resolve => setTimeout(resolve, 1000));
            })
        ])
    ];
    return allTestSuites;
}
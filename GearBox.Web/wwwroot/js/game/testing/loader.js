/**
 * use this to export all the "real" tests used by the program
 */
import { TestCase, TestSuite } from "./tests.js";
import { playerTests } from "../model/player.js";
import { itemTests } from "../model/item.js";

/**
 * @returns TestSuite[]
 */
export function getAllTestSuites() {
    const allTestSuites = [
        new TestSuite("test suite 1", [
            new TestCase("test 1.1", () => {
                console.log("hello world!");
            }),
            new TestCase("test 1.2", (assertions) => {
                assertions.assert(1 + 1 == 2);
            })
        ]),
        new TestSuite("test suite 2", [
            new TestCase("async test", async () => {
                // https://stackoverflow.com/a/39914235
                await new Promise(resolve => setTimeout(resolve, 1000));
            })
        ]),
        itemTests,
        playerTests
    ];
    return allTestSuites;
}
import { Assertions } from "../js/game/testing/assertions.js";
import { getAllTestSuites } from "../js/game/testing/loader.js";

$(async () => await main());

async function main() {
    console.log("loaded tests");
    const allTestSuites = getAllTestSuites();    
    const assertions = new Assertions();
    for (const testSuite of allTestSuites) {
        for (const testCase of testSuite.tests) {
            try {
                await testCase.run(assertions);
                _passed(testSuite, testCase);
            } catch(ex) {
                _failed(testSuite, testCase, ex);
            }
        }
    }
    $("#head").css("background-color", "green");
}

function _passed(testSuite, testCase) {
    const row = `
        <tr>
            <td>${testSuite.name}</td>
            <td>${testCase.name}</td>
            <td>passed</td>
        </tr>
    `;
    $("#output").after(row);
}

function _failed(testSuite, testCase, error) {
    const row = `
        <tr>
            <td>${testSuite.name}</td>
            <td>${testCase.name}</td>
            <td style="background-color:red">failed: ${error.message}</td>
        </tr>
    `;
    $("#output").after(row);
    console.error(error);
}
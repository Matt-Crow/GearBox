/**
 * @param {object} json 
 * @returns {string}
 */
export function getColorStringFromJson(json) {
    const r = json.red;
    const g = json.green;
    const b = json.blue;
    const result = `rgb(${r} ${g} ${b})`; // CSS color doesn't use commas
    // https://developer.mozilla.org/en-US/docs/Web/CSS/color_value/rgb
    return result;
}
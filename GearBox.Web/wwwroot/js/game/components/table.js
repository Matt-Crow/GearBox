export class Table {
    #selector;
    #columns;
    #onRowCreated;

    /**
     * @param {string} selector 
     * @param {Column[]} columns 
     * @param {(record: any, row: HTMLTableRowElement) => any} onRowCreated called when a row is created for a record
     */
    constructor(selector, columns, onRowCreated=null) {
        this.#selector = selector;
        this.#columns = columns;
        this.#onRowCreated = onRowCreated ?? (_ => {});
    }

    spawnHtml() {
        $(this.#selector)
            .empty()
            .append($("<table>").addClass("table table-striped table-hover")
                .append(this.#getTheadHtml())
                .append($("<tbody>"))
            );
    }
        
    #getTheadHtml() {
        const $tr = $("<tr>");
        this.#columns
            .map(col => $("<td>").text(col.headerText))
            .forEach($td => $tr.append($td));
        const $thead = $("<thead>").addClass("thead-dark")
            .append($tr);
        return $thead;
    }

    setRecords(records=[]) {
        const $rows = $(`${this.#selector} tbody`)
            .empty();
        records
            .map(record => this.#createRow(record))
            .forEach($row => $rows.append($row));
    }

    #createRow(record) {
        const $row = $("<tr>");
        
        this.#columns.forEach(col => {
            const cellData = col.makeCellData(record);
            return $row.append($("<td>").append(cellData));
        });

        this.#onRowCreated(record, $row[0]);
        
        return $row;
    }
}

class Column {
    #headerText;

    /**
     * @param {string} headerText 
     */
    constructor(headerText) {
        this.#headerText = headerText;
    }

    get headerText() { return this.#headerText; }

    makeCellData(_data) {
        throw new Error("abstract method");
    }
}

export class DataColumn extends Column {
    #getCellText;

    /**
     * @param {string} headerText 
     * @param {(data: object) => string} getCellText 
     */
    constructor(headerText, getCellText) {
        super(headerText);
        this.#getCellText = getCellText;
    }

    makeCellData(data) {
        return this.#getCellText(data);
    }
}

export class ActionColumn extends Column {
    #buttonText;
    #onClick;

    /**
     * @param {string} headerText 
     * @param {string} buttonText 
     * @param {(data: object) => any} onClick 
     */
    constructor(headerText, buttonText, onClick) {
        super(headerText);
        this.#buttonText = buttonText;
        this.#onClick = onClick;
    }

    makeCellData(data) {
        const $cell = $("<button>")
            .attr("type", "button")
            .addClass("btn btn-primary")
            .text(this.#buttonText)
            .on("click", (_) => this.#onClick(data));
        return $cell;
    }
}

/**
 * button is only enabled when a condition is met
 */
export class ConditionalActionColumn extends ActionColumn {
    #checkCondition;

    constructor(headerText, buttonText, onClick, checkCondition) {
        super(headerText, buttonText, onClick);
        this.#checkCondition = checkCondition;
    }

    makeCellData(data) {
        const result = super.makeCellData(data);
        if (!this.#checkCondition(data)) {
            result.prop("disabled", true);
        }
        return result;
    }
}
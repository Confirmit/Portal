function selectAllEntitiesCheckBoxes(dataGrid, currentCheckBoxElement, isHeader) {
    var inputList = dataGrid.getElementsByTagName("input");
    var headerCheckBox = inputList[0];
    if (isHeader) {
        var rows = dataGrid.rows;
        for (var index = 1; index < rows.length; index++) {
            var currentRow = rows[index];
            var cell = currentRow.cells[0];
            var checkBox = cell.children[0];
            checkBox.checked = headerCheckBox.checked;
        }
    } else {
        if (!currentCheckBoxElement.checked) {
            headerCheckBox.checked = false;
        }
    }
}
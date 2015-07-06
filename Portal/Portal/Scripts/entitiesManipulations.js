function selectAllEntitiesCheckBoxes(entitiesListGridViewClientId, isCheck) {
    var dataGrid = document.getElementById(entitiesListGridViewClientId);
    var rows = dataGrid.rows;
    for (var index = 1; index < rows.length; index++) {
        var currentRow = rows[index];
        var cell = currentRow.cells[0];
        var checkBox = cell.children[0];
        checkBox.checked = isCheck === "true" ? true : false;
    }
}
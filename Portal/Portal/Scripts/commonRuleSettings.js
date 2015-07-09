function selectAllDaysOfWeekCheckBoxes(daysOfWeekCheckBoxListId, selectAllDaysCheckBoxClientId,
    currentCheckBoxClientId) {
    var daysOfWeekCheckBoxList = document.getElementById(daysOfWeekCheckBoxListId);
    var selectAllDaysCheckBox = document.getElementById(selectAllDaysCheckBoxClientId);
    var currentCheckBox = document.getElementById(currentCheckBoxClientId);
    var rows = daysOfWeekCheckBoxList.rows;
    if (currentCheckBox == selectAllDaysCheckBox) {
        for (var index = 0; index < rows.length; index++) {
            var currentRow = rows[index];
            var cell = currentRow.cells[0];
            var checkBox = cell.children[0];
            checkBox.checked = selectAllDaysCheckBox.checked;
        }
    } else {
        if (!currentCheckBox.checked)
            selectAllDaysCheckBoxClientId.checked = false;
    }
}
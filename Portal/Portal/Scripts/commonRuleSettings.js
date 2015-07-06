function selectAllDaysOfWeekCheckBoxes(daysOfWeekCheckBoxListId, selectAllDaysCheckBoxClientId) {
    var daysOfWeekCheckBoxList = document.getElementById(daysOfWeekCheckBoxListId);
    var selectAllDaysCheckBox = document.getElementById(selectAllDaysCheckBoxClientId);
    var state = selectAllDaysCheckBox.checked;
    var checkBoxes = daysOfWeekCheckBoxList.getElementsByTagName("input");
    for (var i = 0; i < checkBoxes.length; i++)
        checkBoxes[i].checked = state;
}
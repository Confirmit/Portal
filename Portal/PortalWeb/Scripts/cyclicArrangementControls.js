//---------------------------------------------------------------------
function cyclicArrangementControls() {
    this.onChange();
}
cyclicArrangementControls.prototype.GetSelectedRadioButtonIndex = function(name) {
    var radioButtons = document.getElementsByName(name);
    for (var i = 0; i < radioButtons.length; i++) {
        if (radioButtons[i].checked)
            return i;
    }
    return -1;
}
cyclicArrangementControls.prototype.onChange = function() {
    name = 'ctl00$MainContentPlaceHolder$rbListDailyWeekly'
    ident = this.GetSelectedRadioButtonIndex(name);
    var daysDiv = document.getElementById('daysDiv');
    var weeksDiv = document.getElementById('weeksDiv');
    if (ident == 0) {
        daysDiv.style.display = 'block';
        weeksDiv.style.display = 'none';
    }
    else {
        daysDiv.style.display = 'none';
        weeksDiv.style.display = 'block';
    }
}
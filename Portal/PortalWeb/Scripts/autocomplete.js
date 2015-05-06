function onKeypress(dropdownlist, event) {
    // check the keypressBuffer attribute is defined on the dropdownlist
    var undefined;
    
    if (dropdownlist.keypressBuffer == undefined) {
        dropdownlist.keypressBuffer = '';
    }
    if (event.keyCode == 13) {
        dropdownlist.onchange();
        return false;
    }
    
    // get the key that was pressed
    var key = String.fromCharCode(event.keyCode);
    dropdownlist.keypressBuffer += key;
    dropdownlist.keypressBuffer = dropdownlist.keypressBuffer.toLowerCase();
    
    // find if it is the start of any of the options
    var optionsLength = dropdownlist.options.length;
    for (var n = 0; n < optionsLength; n++) {
        var optionText = dropdownlist.options[n].text;
        optionText = optionText.toLowerCase();

        if (optionText.indexOf(dropdownlist.keypressBuffer, 0) == 0) {
            dropdownlist.selectedIndex = n;
            return false; // cancel the default behavior since
            // we have selected our own value
        }
    }

    // reset initial key to be inline with default behavior
    dropdownlist.keypressBuffer = key;
    return true; // give default behavior
}



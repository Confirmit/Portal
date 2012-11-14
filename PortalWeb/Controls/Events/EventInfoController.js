//JS file
function EventInfoController(clientObjectName, userHeader, groupHeader)
{
    this.cbUserHeader = userHeader;    
    this.cbGroupHeader = groupHeader;
    
    this.clientObjectName = clientObjectName;
}

EventInfoController.prototype.InitCBLists = function(cbUsersList, cbRolesList) {
    var cbUserHeader = $get(this.cbUserHeader);
    var users_enable = cbUserHeader.checked &&
        (cbUserHeader.getAttribute("disabled") == null
        || cbUserHeader.getAttribute("disabled") == false);
        
    this.SetEnablePropertyToChilds(cbUsersList, users_enable);

    var cbGroupHeader = $get(this.cbGroupHeader);
    var roles_enable = cbGroupHeader.checked &&
        (cbGroupHeader.getAttribute("disabled") == null
        || cbGroupHeader.getAttribute("disabled") == false);

    this.SetEnablePropertyToChilds(cbRolesList, roles_enable);
}

EventInfoController.prototype.CheckBoxes_OnClick = function(checkBox, cbListID) {
    var cbUserHeader = document.getElementById(this.cbUserHeader);
    var cbGroupHeader = document.getElementById(this.cbGroupHeader);

    if (!cbUserHeader.checked && !cbGroupHeader.checked)
        checkBox.checked = true;

    this.SetEnablePropertyToChilds(cbListID, checkBox.checked);
}

EventInfoController.prototype.SetEnablePropertyToChilds = function(cbListID, value) {
    var cbList = $get(cbListID);
    if (cbList == null)
        return;

    for (var childItem in cbList.childNodes) {
        this.setEnablePropertyToChild(cbList.childNodes[childItem], value);
    }

    return false;
}

EventInfoController.prototype.setEnablePropertyToChild = function(object, value) {
    if (object.nodeType != 1)
        return;

    if (object.childNodes.length == 0) {
        if (object.type == 'checkbox') {
            if (value)
                object.removeAttribute("disabled");
            else
                object.setAttribute("disabled", "disabled");
                
            return;
        }

        return;
    }

    for (var childItem in object.childNodes)
        this.setEnablePropertyToChild(object.childNodes[childItem], value);

    return;
}
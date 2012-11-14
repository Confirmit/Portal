// JScript File
var obj;

function SetParameters(contolRegDateId, contolCloseDateId, idElemMenu) {
    obj = new param(contolRegDateId, contolCloseDateId, idElemMenu);
}

function param(contolRegDateId, contolCloseDateId, idElemMenu) {
    this.contolRegDateId = contolRegDateId;
    this.contolCloseDateId = contolCloseDateId;
    this.HIDDEN_ACTION_NAME = idElemMenu + '_action';
}
 
ActionsMenuController.prototype.OnSave =  function()
{
    var hiddenASkToNewModification = document.getElementById("ctl00_PageActionsPlaceHolder_hiddenASkToNewModification");
    if (hiddenASkToNewModification == 'null')
    {
        $get(obj.HIDDEN_ACTION_NAME).value = 'save';
        formSubmiting();    
    }

    if (document.aspnetForm.ctl00$data_change.value == 'change' && hiddenASkToNewModification.value != '')
        Ext.MessageBox.confirm(getDialogTitleMessage(),hiddenASkToNewModification.value, OnSaveConfirm);
    else OnSaveConfirm(Ext.MessageBox.buttonText.yes.toLowerCase());
}

function OnSaveConfirm(btn){
    if (btn == Ext.MessageBox.buttonText.yes.toLowerCase()){
        var datePickerRegistrationDate = document.getElementById(obj.contolRegDateId);
        if (datePickerRegistrationDate.value == '')
        {
            Ext.Msg.alert(getDialogTitleMessage(), 'Please, sure that you input Registration Date!');
            return;
        }
        
        var datePickerClosingDate = document.getElementById(obj.contolCloseDateId);
        
        if (datePickerClosingDate.value != '')
        {
            var dtR = fromStringToDate(datePickerRegistrationDate.value);
            var dtC = fromStringToDate(datePickerClosingDate.value);
            
            if (dtR > dtC)
            {
                Ext.Msg.alert(getDialogTitleMessage(), 'Please, check than Registration Date is earlier than Closing Date.');
                return;
            }
        }
        $get(obj.HIDDEN_ACTION_NAME).value = 'save';
        formSubmiting();
    }
}


//---------------------------------------------------------------------

function ActionProcessor(idParentElem, objDataChangedElem)
{
    this.idParentElem = idParentElem;
    this.objDataChangedElem = objDataChangedElem;

    this.HIDDEN_ACTION_NAME = this.idParentElem + '_action';
    this.HIDDEN_ACTION_NAVIGATE = this.idParentElem + '_navigate';
    this.HIDDEN_ACTION_PARAMETER = this.idParentElem + '_parameter';

    //Post back event params
    this.eventTarget = '';
    this.eventArgument = '';

    this._formRequestManager = null;
}

ActionProcessor.ACTION_ADD = 'add';
ActionProcessor.ACTION_SAVE = 'save';
ActionProcessor.ACTION_SAVE_AND_NEW = 'save_and_new';
ActionProcessor.ACTION_SAVE_AS_NEW = 'savenew';
ActionProcessor.ACTION_DELETE = 'delete';
ActionProcessor.ACTION_CLOSE = 'close';
ActionProcessor.ACTION_REFRESH = 'refresh';

ActionProcessor.ACTION_MOVE = 'move';
ActionProcessor.ACTION_COPY = 'copy';
ActionProcessor.ACTION_UNLINK = 'unlink';

ActionProcessor.prototype.initHiddenElem = function(strElemName)
{
    var hiddenElem = $get(strElemName);
    if (hiddenElem == null) {
        hiddenElem = document.createElement('input');
        hiddenElem.type = 'hidden';
        hiddenElem.id = strElemName;
        hiddenElem.name = strElemName;
        
        $get(this.idParentElem).appendChild(hiddenElem);
    }
    else {
        hiddenElem.value = '';
    }
}

ActionProcessor.prototype.SetFormRequestManager = function(formRequestManager)
{
    this._formRequestManager = formRequestManager;
}

ActionProcessor.prototype.SetAjaxRequestControl = function(objAjaxRequestControl)
{
    this.objAjaxRequestControl = objAjaxRequestControl;
}

ActionProcessor.prototype.SetEventParams = function(eventTarget, eventArgument)
{
    this.eventTarget = eventTarget;
    this.eventArgument = eventArgument;
}

ActionProcessor.prototype.SetActionValues = function(action_name, navigate_to_view)
{
    this.initHiddenElem(this.HIDDEN_ACTION_NAME);
    this.initHiddenElem(this.HIDDEN_ACTION_NAVIGATE);

    if (typeof(navigate_to_view) != 'undefined')
        $get(this.HIDDEN_ACTION_NAVIGATE).value = navigate_to_view;        
    
    $get(this.HIDDEN_ACTION_NAME).value = action_name;
}

ActionProcessor.prototype.SetActionParameter = function(action_parameter)
{
    this.initHiddenElem(this.HIDDEN_ACTION_PARAMETER);
    
    $get(this.HIDDEN_ACTION_PARAMETER).value = action_parameter;
}

ActionProcessor.prototype.DoActionSilently = function(action_name, navigate_to_view)
{
    this.SetActionValues(action_name, navigate_to_view);

    var fUseAjaxRequestManager = (typeof(this.objAjaxRequestControl) != 'undefined' && this.objAjaxRequestControl != null);

    if (fUseAjaxRequestManager) {
        if (this.objAjaxRequestControl.CheckRequest()) 
            return;

        this.objAjaxRequestControl.StartRequest();
    }
    
    if (this._formRequestManager !== null)
        this._formRequestManager._doPostBack(this.eventTarget, this.eventArgument);
    else
        __doPostBack(this.eventTarget, this.eventArgument);
}

ActionProcessor.prototype.DoAction = function(action_name, navigate_to_view)
{
    if (typeof(this.objDataChangedElem) !== 'undefined' && this.objDataChangedElem !== null
     && this.objDataChangedElem.value != '')
    {
        this.current_action_name = action_name;
        this.current_navigate_to_view = navigate_to_view;
        
        Ext.MessageBox.confirm(getDialogTitleMessage(), 'Some data are modified and not saved.<br>Do you want to continue without saving?', 
            this.onActionConfirm, this);
    }
    else {
        this.DoActionSilently(action_name, navigate_to_view);
    }
}

ActionProcessor.prototype.onActionConfirm = function(btn)
{
    if (btn == Ext.MessageBox.buttonText.yes.toLowerCase()) 
    {
        this.DoActionSilently(this.current_action_name, this.current_navigate_to_view);
    }

    this.current_action_name = '';
    this.current_navigate_to_view = '';
}

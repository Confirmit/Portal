function ActionsMenuController(idElemMenu, nameTargetControl) {
	this.idElemMenu = idElemMenu;
	this.nameTargetControl = nameTargetControl;
	this._currentAction = null;
	this._nextViewToNavigate = null;
	
	this.objProcessedGrid = null;
	this.objMenuDeleteItem = null;
	this.objMenuCloseItem = null;
		
	this.MSG_DELETE_CONFIRM = "You are about to delete the selected entry(ies). If you click Yes, you won’t be able to undo this Delete operation. Are you sure you want to delete these entry(ies)?";
	this.MSG_ACTION_CONFIRM = "Are you sure you want to do this action?";
}

//-----------------------------------------------------------------------------------
// Process disabling of menu items
//

ActionsMenuController.prototype.SetProcessedGrid = function(objProcessedGrid) {
	if (typeof (objProcessedGrid) == 'undefined' || objProcessedGrid == null) {
		this.processDeleteItemState(false);
		return;
	}

	this.objProcessedGrid = objProcessedGrid;
	this.objProcessedGrid.AddListener(this.OnGridEvent, this
	, this.objProcessedGrid.ONCHECK_EVENT_NAME);

	if (this.objProcessedGrid != null)
		this.processDeleteItemState(this.objProcessedGrid.IsCheckedAnyRow());
}

ActionsMenuController.prototype.SetMenuDeleteItem = function(objMenuDeleteItem)
{
	this.objMenuDeleteItem = objMenuDeleteItem;

	if (this.objProcessedGrid != null)
		this.processDeleteItemState(this.objProcessedGrid.IsCheckedAnyRow());    
}

ActionsMenuController.prototype.SetMenuCloseItem = function(objMenuCloseItem) 
{
	this.objMenuCloseItem = objMenuCloseItem;

	if (this.objProcessedGrid != null)
		this.processDeleteItemState(this.objProcessedGrid.IsCheckedAnyRow());
}

ActionsMenuController.prototype.OnGridEvent = function(isCheckedAnyRow) {
	this.processDeleteItemState(isCheckedAnyRow);
}

ActionsMenuController.prototype.processDeleteItemState = function(isCheckedAnyRow) {
	if (isCheckedAnyRow === true) {
		if (typeof (this.objMenuDeleteItem) !== "undefined"
		 && this.objMenuDeleteItem !== null)
			this.objMenuDeleteItem.jdEnable();

		if (typeof (this.objMenuCloseItem) !== "undefined"
		 && this.objMenuCloseItem !== null)
			this.objMenuCloseItem.jdEnable();
	}
	else {
		if (typeof (this.objMenuDeleteItem) !== "undefined"
		 && this.objMenuDeleteItem !== null)
			this.objMenuDeleteItem.jdDisable();

		if (typeof (this.objMenuCloseItem) !== "undefined"
		 && this.objMenuCloseItem !== null)
			this.objMenuCloseItem.jdDisable();
	}
}

//-----------------------------------------------------------------------------------
// Process commands
//

ActionsMenuController.prototype.OnSilentAction = function(action, param) 
{
	var action_processor = new ActionProcessor(this.idElemMenu);
	if (typeof(this.nameTargetControl) != 'undefined') {
		action_processor.SetEventParams(this.nameTargetControl, '');
	}
	action_processor.SetActionParameter(param);
	action_processor.DoActionSilently(action);
}

ActionsMenuController.prototype.OnAddNew = function() 
{
	var action_processor = new ActionProcessor(this.idElemMenu);//, document.aspnetForm.ctl00$data_change);
	
	if (typeof(this.nameTargetControl) != 'undefined') {
		action_processor.SetEventParams(this.nameTargetControl, '');
	}
	action_processor.DoAction(ActionProcessor.ACTION_ADD);
}

ActionsMenuController.prototype.OnAddNewNavigate = function(strNextView) 
{
	var action_processor = new ActionProcessor(this.idElemMenu);//, document.aspnetForm.ctl00$data_change);

	if (typeof(this.nameTargetControl) != 'undefined') {
		action_processor.SetEventParams(this.nameTargetControl, '');
	}
	action_processor.DoAction(ActionProcessor.ACTION_ADD, strNextView);
}

ActionsMenuController.prototype.OnNavigate = function(strNextView, strActionParameter) 
{
	var action_processor = new ActionProcessor(this.idElemMenu);//, document.aspnetForm.ctl00$data_change);

	if (typeof(this.nameTargetControl) != 'undefined') {
		action_processor.SetEventParams(this.nameTargetControl, '');
	}
	action_processor.SetActionParameter(strActionParameter);
	action_processor.DoAction(ActionProcessor.ACTION_ADD, strNextView);
}

ActionsMenuController.prototype.OnSave = function() 
{
	this._currentAction = ActionProcessor.ACTION_SAVE;
	this._doCurrentAction();
}

ActionsMenuController.prototype.OnSaveAndNew = function() 
{
	this._currentAction = ActionProcessor.ACTION_SAVE_AND_NEW;
	this._doCurrentAction();
}

ActionsMenuController.prototype.OnSaveNavigate = function(strNextView) 
{
	this._nextViewToNavigate = strNextView;
	this._currentAction = ActionProcessor.ACTION_SAVE;
	this._doCurrentAction();
}

ActionsMenuController.prototype.OnSaveAndNewNavigate = function(strNextView) 
{
	this._nextViewToNavigate = strNextView;
	this._currentAction = ActionProcessor.ACTION_SAVE_AND_NEW;
	this._doCurrentAction();
}

ActionsMenuController.prototype.OnSaveAsNew = function() 
{
	this._currentAction = ActionProcessor.ACTION_SAVE_AS_NEW;
	this._doCurrentAction();
}

ActionsMenuController.prototype._doCurrentAction = function()
{
	var action_processor = new ActionProcessor(this.idElemMenu);
	if (typeof(this.nameTargetControl) != 'undefined') {
		action_processor.SetEventParams(this.nameTargetControl, '');
	}
	action_processor.DoActionSilently(this._currentAction);

	this._currentAction = null;
	this._nextViewToNavigate = null;
}

ActionsMenuController.prototype.OnDelete = function() 
{
	if (this.objProcessedGrid == null) 
	{
		if (confirm(this.MSG_DELETE_CONFIRM)) 
			this.onDeleteConfirm();

		return;
	}
	
	if (this.objProcessedGrid.IsCheckedAnyRow() == true) 
	{
		if (confirm(this.MSG_DELETE_CONFIRM)) 
			this.onDeleteConfirm();
				
		return;
	}
}

ActionsMenuController.prototype.onDeleteConfirm = function() 
{
	var action_processor = new ActionProcessor(this.idElemMenu);
	
	if (typeof(this.nameTargetControl) != 'undefined') {
		action_processor.SetEventParams(this.nameTargetControl, '');
	}
	
	action_processor.DoActionSilently(ActionProcessor.ACTION_DELETE);
}

ActionsMenuController.prototype.OnAction = function(actionName) 
{		
	if (this.objProcessedGrid == null) 
	{
		if (confirm(this.MSG_ACTION_CONFIRM)) 
			this.onActionConfirm(actionName);

		return;
	}
	
	if (this.objProcessedGrid.IsCheckedAnyRow() == true) 
	{
		if (confirm(this.MSG_ACTION_CONFIRM)) 
			this.onActionConfirm(actionName);
	
		return;
	}
}

ActionsMenuController.prototype.onActionConfirm = function(actionName) 
{
	var action_processor = new ActionProcessor(this.idElemMenu);
		
	if (typeof(this.nameTargetControl) != 'undefined') {
		action_processor.SetEventParams(this.nameTargetControl, '');
	}
		
	action_processor.DoActionSilently(actionName);
}

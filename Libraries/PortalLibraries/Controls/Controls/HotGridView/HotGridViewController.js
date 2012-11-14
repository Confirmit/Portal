// JScript File
function HotGridViewController(gridViewClient, eventTargetId) {
	this._gridViewClient = gridViewClient;
	this.eventTargetId = eventTargetId;
	
	this.HIDDEN_OBJECT_ID = this._gridViewClient.GridId + '_ID';
	this.HIDDEN_OBJECT_INDEX = this._gridViewClient.GridId + '_Index';
	this.HIDDEN_OBJECT_BKEY = this._gridViewClient.GridId + '_BusinessKey';
}

HotGridViewController.prototype.CreateHiddenField = function(GridId, objectId)
{
	if($get(GridId) == null)
		return;
	
	var hiddenField = $get(objectId);
	if (hiddenField == null){
		hiddenField = document.createElement('input');
		hiddenField.type = 'hidden';
		hiddenField.id = objectId;
		hiddenField.name = objectId;

		$get(GridId).appendChild(hiddenField);
	}
	else {
		hiddenField.value = '';
	}
	return hiddenField;
}
	
HotGridViewController.prototype.OnClick =  function(columnIndex, databaseId, businessKey, doSubmit)
{       
	this.hiddenId = $get(this.HIDDEN_OBJECT_ID);
	if (this.hiddenId == null) {
		this.hiddenId = this.CreateHiddenField(this._gridViewClient.GridId, this.HIDDEN_OBJECT_ID);
	}
	this.hiddenId.value = databaseId;
		
	this.hiddenType = $get(this.HIDDEN_OBJECT_INDEX); 
	if (this.hiddenType == null) {
		this.hiddenType = this.CreateHiddenField(this._gridViewClient.GridId, this.HIDDEN_OBJECT_INDEX);
	}
	this.hiddenType.value = columnIndex;
	
	this.hiddenBKey = $get(this.HIDDEN_OBJECT_BKEY); 
	if (this.hiddenBKey == null) {
		this.hiddenBKey = this.CreateHiddenField(this._gridViewClient.GridId, this.HIDDEN_OBJECT_BKEY);
	}
	this.hiddenBKey.value = businessKey;
	
	this._gridViewClient.setSelectedDatabaseId(databaseId);
	this._gridViewClient.setSelectedBusinessKey(businessKey);
	
	if (!doSubmit)
		return;    
	
	// encapsulate this functionality into command
	// with correct event target id
	if (typeof(this.eventTargetId) !== 'undefined')
		__doPostBack(this.eventTargetId, '');
	else
		__doPostBack(this._gridViewClient.GridId, '');
}

HotGridViewController.prototype.OnSelectRowSubmit =  function(columnIndex, databaseId, businessKey)
{       
	this.hiddenId = $get(this.HIDDEN_OBJECT_ID);
	if (this.hiddenId == null) {
		this.hiddenId = this.CreateHiddenField(this._gridViewClient.GridId, this.HIDDEN_OBJECT_ID);
	}
	this.hiddenId.value = databaseId;
		
	this.hiddenType = $get(this.HIDDEN_OBJECT_INDEX); 
	if (this.hiddenType == null) {
		this.hiddenType = this.CreateHiddenField(this._gridViewClient.GridId, this.HIDDEN_OBJECT_INDEX);
	}
	this.hiddenType.value = columnIndex;
	
	this.hiddenBKey = $get(this.HIDDEN_OBJECT_BKEY); 
	if (this.hiddenBKey == null) {
		this.hiddenBKey = this.CreateHiddenField(this._gridViewClient.GridId, this.HIDDEN_OBJECT_BKEY);
	}
	this.hiddenBKey.value = businessKey;
	
	this._gridViewClient.setSelectedDatabaseId(databaseId);
	this._gridViewClient.setSelectedBusinessKey(businessKey);
	
   /* if (document.aspnetForm.ctl00$data_change.value != '') 
	{
		Ext.MessageBox.confirm(getDialogTitleMessage(), 'Some data are modified and not saved.<br>Do you want to continue without saving?', 
			function(btn){
				if (btn == Ext.MessageBox.buttonText.yes.toLowerCase()) {
					document.aspnetForm.ctl00$data_change.value = '';

					__doPostBack(this._gridViewClient.GetUniqueId(), '');
				}
				else {
					this.hiddenId.value = '';
					this.hiddenType.value = '';
					this.hiddenBKey.value = '';
				}              
			}, this
		);
	}
	else {*/
		__doPostBack(this._gridViewClient.GetUniqueId(), '');
	//} 
}
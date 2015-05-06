// JScript File
HotGridView = function(GridId, CheckBoxSpecialId) 
{
    this.GridId = GridId;
    this.CheckBoxSpecialId = CheckBoxSpecialId;

    this._databaseId = 0;
    this._businessKey = 0;
    this._uniqueId = 0;
    
    this.listeners = new Array();
    this.contexts = new Array();
    this.events = new Array();
    this.ONCHECK_EVENT_NAME = "oncheck";
}

HotGridView.prototype.SetUniqueId = function(id)
{
    this._uniqueId = id;
}

HotGridView.prototype.GetUniqueId = function()
{
    return this._uniqueId;
}

HotGridView.prototype.setSelectedDatabaseId = function(id)
{
    this._databaseId = id;
}

HotGridView.prototype.getSelectedDatabaseId = function()
{
    return this._databaseId;
}

HotGridView.prototype.setSelectedBusinessKey = function(key)
{
    this._businessKey = key;
}

HotGridView.prototype.getSelectedBusinessKey = function()
{
    return this._businessKey;
}

HotGridView.prototype.AddListener = function(handler, context, event_name)
{
    this.listeners[this.listeners.length] = handler;
    this.contexts[this.contexts.length] = context;    
    this.events[this.events.length] = event_name;
}
   
HotGridView.prototype.IsCheckedAnyRow =  function()
{       
    var grid = $get(this.GridId); 
    
    if (grid == null)
        return;
    
    for (var childItem in grid.childNodes)
    {
        if(this.isHaveSpecialCheckedChild(grid.childNodes[childItem]))
            return true;
    }
    
    return false;
}

HotGridView.prototype.isHaveSpecialCheckedChild = function(object)
{
    if (object.nodeType != 1)
        return false;
    
    if (object.childNodes.length == 0) {
        
        if (object.type == 'checkbox' && object.id.indexOf(this.CheckBoxSpecialId) != -1
         && object.checked)
            return true;
        
        return false;
    }
        
    for (var childItem in object.childNodes)
        if (this.isHaveSpecialCheckedChild(object.childNodes[childItem]))
            return true;
    
    return false;
}

HotGridView.prototype.CheckAll = function(me)
{
// '$HeaderButton' доолжен быть такой же как и в cs коде!!!
    //var me = $get(this.GridId);
    var index = me.name.indexOf('$HeaderButton');
    var prefix = me.name.substr(0,index); 
    
    for(i = 0; i < document.forms[0].length; i++) 
    { 
        var o = document.forms[0][i]; 
        if (o.type == 'checkbox') 
        { 
            if (me.name != o.name) 
            {
                if (o.name.substring(0, prefix.length) == prefix) 
                {
                    o.checked = !me.checked; 
                    o.click(); 
                }
            }
        }
    }
    
    this.fireEvent(this.ONCHECK_EVENT_NAME);
}

HotGridView.prototype.ApplyStyle = function(me, selectedClassName, selectedForeColor, selectedBackColor, selectedBold, className, foreColor, backColor, bold, checkBoxHeaderId) 
{ 
    var td = me.parentNode; 
    if (td == null) 
        return; 
      
    var tr = td.parentNode;
    if (me.checked)
    { 
       tr.style.fontWeight = selectedBold; // bold
       tr.style.color = selectedForeColor; 
       tr.style.backgroundColor = selectedBackColor; 
       tr.className = selectedClassName;
    } 
    else 
    { 
       document.getElementById(checkBoxHeaderId).checked = false;
       tr.style.fontWeight = bold; 
       tr.style.color = foreColor; 
       tr.style.backgroundColor = backColor; 
       tr.className = className; 
    } 

    this.fireEvent(this.ONCHECK_EVENT_NAME);
}

HotGridView.prototype.fireEvent = function(event_name)
{
    // fire event
    for (var i = 0; i < this.listeners.length; i++)
    {
        if (event_name == this.events[i])        
            this.listeners[i].call(this.contexts[i], this.IsCheckedAnyRow());
    }
}
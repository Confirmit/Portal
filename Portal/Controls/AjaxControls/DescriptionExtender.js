/*function DescriptionExtender(idParentElem, strElemName, clientObjectName)
{    
    this.idParentElem = idParentElem;
    this.idElemName = strElemName;
    
    this.clientObjectName = clientObjectName;
    this.currentID = 0;
    this.initFileUploader();
}*/

// Move an element directly on top of another element (and optionally
// make it the same size)
doCover = function(bottomControlID, topControlID, ignoreSize)
{
    var bottom = $get(bottomControlID);
    var top = $get(topControlID);
        
    var location = Sys.UI.DomElement.getLocation(bottom);
    top.style.position = 'absolute';
    top.style.top = location.y + 'px';
    top.style.left = location.x + 'px';
        
    if (!ignoreSize) 
    {
        top.style.height = bottom.offsetHeight + 'px';
        top.style.width = bottom.offsetWidth + 'px';
    }
}

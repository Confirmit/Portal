var curItem;
var selItem;
function init(){
	document.onmouseup = function(){
		mouseup();  
	}
}
function overItem(item) {
    if (item == null)
        return;
    var browser_name = navigator.appName;
    var browser_version = parseFloat(navigator.appVersion);
    if (browser_name == "Microsoft Internet Explorer" && browser_version <= 6.0){
	    item.className = "menu-item-highlight";	
	    check(item);
	}
}
function outItem(item) {
    var browser_name = navigator.appName;
    var browser_version = parseFloat(navigator.appVersion);
    if (browser_name == "Microsoft Internet Explorer" && browser_version <= 6.0)
	    item.className = "menu-item";
}
function outSubTable(thisitem, ID){
    thisitem.style.visibility = 'hidden';	
}
function overRoot(parent, _item, _nested, ID) {
	var item = document.getElementById(_item);
	if (item == null || parent == null)
	    return;
	parent.className = "menu-item-highlight";
	check(parent, ID);
	
    var offsetLeft = 0;
    var offsetTop = 0;
	offsetTrail = parent;
	while(offsetTrail != null && offsetTrail.style.position != "absolute")
    {
        offsetLeft += offsetTrail.offsetLeft;
        offsetTop += offsetTrail.offsetTop;
        offsetTrail = offsetTrail.offsetParent;
    }
    if ((navigator.userAgent.indexOf("Mac") != -1)&&(typeof document.body.leftMargin != "undefined"))
    {
        offsetLeft += document.body.leftMargin;
        offsetTop += document.body.topMargin;
    }
    offsetTop += parent.offsetHeight;
	item.style.top  = (offsetTop + 'px');
	item.style.left  = (offsetLeft + 'px');
	item.style.visibility = 'visible';
	curItem = item;
}
function outRoot(parent, ID){
    parent.className = "menu-item";
    var browser_name = navigator.appName;
    var browser_version = parseFloat(navigator.appVersion);
    if (browser_name == "Microsoft Internet Explorer" && browser_version >= 4.0){
        if (window.event.offsetX < 0 || window.event.offsetX > parent.offsetWidth)
            mouseup(ID);
        if (window.event.offsetY < 0){
            mouseup(ID);
        }
    }
}
function check(item, ID){
	if (item.parentNode.parentNode.parentNode.id == ID)
		mouseup(ID);
}
function mouseup(ID) {
	if (curItem != null)
	{
	    var str = "HiddenPlace" + ID;
        var _hidden = document.getElementById(str);
		if (_hidden != null)
		{	
		    var childrens;
		    if (navigator.appName == "Netscape")
                childrens = _hidden.childNodes;
            else childrens = _hidden.children;
			for (var i = 0; i < childrens.length; i++)
				childrens.item(i).style.visibility = 'hidden';
		}
		curItem = null;
	}
}
init();	
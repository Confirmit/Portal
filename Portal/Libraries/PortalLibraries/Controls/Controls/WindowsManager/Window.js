//Fix it!!!
//Type.registerNamespace("IEMS");
//Type.registerNamespace("IEMS.WebForms");

IEMS = function() {}
IEMS.WebForms = function() {}

if (typeof(IEMS.WebForms.Window) === "undefined")
{

IEMS.WebForms.Window = function()
{
    this._window = null;
    this._windowBodyPanel = null;

    this._idElemAnimate = '';

    this._formRequestManager = null;
}

IEMS.WebForms.Window.prototype = 
{
    dispose: function()
    {
        if (this._windowBodyPanel !== null) {
            this._windowBodyPanel.destroy();
            this._windowBodyPanel = null;
        }

        if (this._window !== null) {
            this._window.destroy();
            this._window = null;
        }
       
        if (this._formRequestManager !== null) {
            this._formRequestManager.dispose();
            this._formRequestManager = null;
        }    
    },
    
    isLoaded: function()
    {
        return (typeof(this._formRequestManager) !== 'undefined' && 
                this._formRequestManager !== null);
    },
    
    setButtonsAlign: function(buttonAlign)
    {
        if (this._window === null)
            return;

        this._window.buttonAlign = buttonAlign;
    },

    addButton: function(button, handler, scope)
    {        
        if (this._window === null)
            return;
        
        this._window.addButton(button, handler, scope);
    },

    setResizable: function(resizable)
    {
        if (this._window === null)
            return;

        this._window.resizable = resizable;
    },

    setSize: function(wndWidth, wndHeight)
    {
        if (this._window === null)
            return;
        
        this._window.width = wndWidth;
        this._window.height = wndHeight;         
    },
    
    setTitle: function(wndTitle)
    {
        if (this._window === null)
            return;

        this._window.title = wndTitle;
    },
    
    setCloseAction: function(action)
    {
        if (this._window === null)
            return;
        
        this._window.closeAction = action;
    },
    
    doModal: function(idElemAnimate)
    {
        if (this._window === null)
            return;

        this._window.modal = true;
        this._idElemAnimate = (typeof(idElemAnimate) === 'undefined') 
            ? '' : idElemAnimate;

        this._window.show(this._idElemAnimate);
    },
    
    hideWindowAction: function()
    {
        if (this._window === null)
            return;

        this._window.hide(this._idElemAnimate, null, null);
    },
    
    //private
    _onCloseWindow: function()
    {
        this.dispose();
    },
    
    //private
    _onDeactivateWindow: function()
    {
        if (this._formRequestManager !== null) {
            this._formRequestManager.deactivate();
        }
    },

    //private
    _onActivateWindow: function()
    {
        if (this._formRequestManager !== null) {
            this._formRequestManager.activate();
        }
    }

    ///TODO window activation/deactivation processing
}

//Fix it!!
//IEMS.WebForms.Window.registerClass("IEMS.WebForms.Window", Sys.EventArgs);

}

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
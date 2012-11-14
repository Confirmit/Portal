if (typeof(IEMS.WebForms.WindowsManager) === "undefined")
{

IEMS.WebForms.WindowsManager = function()
{
    this._processingWnd = null;
    this._processingFormRequestManagerName = null;
    this._processingObjects = [];
    this._processingTypes = [];
}

IEMS.WebForms.WindowsManager.prototype = 
{
    createWindow: function()
    {
        var wnd = new IEMS.WebForms.Window();
    
        wnd._windowBodyPanel = new Ext.Panel({
            autoScroll: true,
            collapsible: true,
            margins:'3 0 3 3',
            cmargins:'3 3 3 3',
            border:false
        });

        wnd._window = new Ext.Window({
            layout: 'fit',
            plain: true,
            border: false,
            resizable: false,
            buttonAlign: 'right',
            items: wnd._windowBodyPanel
        });

        wnd._window.on('close', wnd._onCloseWindow, wnd);
        wnd._window.on('deactivate', wnd._onDeactivateWindow, wnd);
        wnd._window.on('activate', wnd._onActivateWindow, wnd);
    
        return wnd;
    },

    loadWindow: function(wnd, url)
    {
        if (this._processingWnd !== null)
            return;
    
        this._processingWnd = wnd;
        
        if (typeof(wnd._window.buttons) !== "undefined") {
            Array.forEach(wnd._window.buttons, function(elt) {
                    elt.disable();
                }
            );        
        }
        
        Sys._ScriptLoader.readLoadedScripts();

        wnd._windowBodyPanel.load({
            url: url,
            scripts: false,
            text: "Loading content...",
            callback: this._loadCallback,
            scope: this
        });
    },

    //private
    _loadCallback: function(element, fSuccess, response) 
    {
        if (fSuccess) {
            this._processLoadedScripts(element)
        }
        else {        
            element.update(response.responseText);
        }
    },

    //private
    _processLoadedScripts: function(element)
    {
        var scriptLoader = Sys._ScriptLoader.getInstance();

        var re = /(?:<script([^>]*)?>)((\n|\r|.)*?)(?:<\/script>)/ig;
        var srcRe = /\ssrc=([\'\"])(.*?)\1/i;

        var match;
        while (match = re.exec(element.dom.innerHTML)) {
            var attrs = match[1];
            var srcMatch = attrs ? attrs.match(srcRe) : false;
            
            if (srcMatch && srcMatch[2]) {
                var scriptSource = srcMatch[2].replace("&amp;", "&");
                if (!Sys._ScriptLoader.isScriptLoaded(scriptSource)) {
                    scriptLoader.queueScriptReference(scriptSource);
                }
            }
            else if (match[2] && match[2].length > 0) {
                var script = match[2];
                
                if (script.indexOf("theForm") >= 0)
                    continue;

                //fix for IE eval - comments should be deleted.
                var reComments = /<!\-\-((\r|\n|.)*?)\-\->/
                var matchComments = reComments.exec(script);
                if (matchComments != null && matchComments.length > 0)
                    script = matchComments[1];

                scriptLoader.queueScriptBlock(script);

                // Get the form request manager from the loading page
                var reRequestManager = /[; \r\n]+(var[ \r\n]+)?([^ \r\n]*)[ \r\n]*=[ \r\n]*Sys\.WebForms\.PageRequestManager\._initialize/
                var matchRequestManager = reRequestManager.exec(script);
                if (matchRequestManager !== null && matchRequestManager.length > 0) {
                    this._processingFormRequestManagerName = matchRequestManager[2];
                }
                
                //Process other client objects from loading page
                var reClientObject = /[; \r\n]+(var[ \r\n]+)?([^ \r\n]*)[ \r\n]*=[ \r\n]*new[ \r\n]*([^ \r\n\(]*)/g
                var matchClientObject;
                while (matchClientObject = reClientObject.exec(script)) {
                    Array.add(this._processingObjects, matchClientObject[2]);
                    Array.add(this._processingTypes, matchClientObject[3]);                    
                }
            }
        }
        
        element.dom.innerHTML = element.dom.innerHTML.replace(
            /(?:<script.*?>)((\n|\r|.)*?)(?:<\/script>)/ig, "");

        scriptLoader.loadScripts(0, Function.createDelegate(this, this._scriptsLoadComplete), null, null);
    },

    //private
    _scriptsLoadComplete: function()
    {
        // process other loaded objects and embed them into the window object
        Array.forEach(this._processingObjects, function(elt, i, arrObjectNames) {
                var object = eval(elt);

                //embed by type
                this._processingWnd[this._processingTypes[i]] = object;
                //embed by name
                this._processingWnd[arrObjectNames[i]] = object;
            }
          , this
        );

        // process created JS objects and add them to the current window
        this._processingWnd._formRequestManager = eval(this._processingFormRequestManagerName);

        if (this._processingWnd.isLoaded()) {
            if (typeof(this._processingWnd._window.buttons) !== "undefined") {
                Array.forEach(this._processingWnd._window.buttons, function(elt) {
                        elt.enable();
                    }
                );
            }
        }
                
        //TODO: raise 'load' event here to launch all load handlers
    
        this._processingWnd = null;
        this._processingFormRequestManagerName = null;
        this._processingObjects = [];
        this._processingTypes = [];
    }
}

//Fix it!!
//IEMS.WebForms.Window.registerClass("IEMS.WebForms.WindowsManager", Sys.EventArgs);

}

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
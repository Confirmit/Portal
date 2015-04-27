// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

Type.registerNamespace("Controls");

Controls.SimpleTabContainer = function(element)
{
    Controls.SimpleTabContainer.initializeBase(this, [element]);
    
    this._cachedActiveTabIndex = -1;
    this._activeHeaderIndex = -1;
    this._tabs = null;
    this._header = null;
    this._body = null;
    this._loaded = false;
    this._autoPostBackId = null;
    
    this._app_onload$delegate = Function.createDelegate(this, this._app_onload);
}

Controls.SimpleTabContainer.prototype = {
    add_activeTabChanged: function(handler) {
        this.get_events().addHandler("activeHeaderChanged", handler);
    },
    remove_activeTabChanged: function(handler) {
        this.get_events().removeHandler("activeHeaderChanged", handler);
    },
    raiseActiveTabChanged: function() {
        var eh = this.get_events().getHandler("activeHeaderChanged");
        if (eh)
            eh(this, Sys.EventArgs.Empty);

        this.set_clientStateFieldValue(this.saveClientState());

        if (this._autoPostBackId)
            __doPostBack(this._autoPostBackId, "activeHeaderChanged:" + this.get_activeHeaderIndex());
    },

    get_activeHeaderIndex: function() {
        if (this._cachedActiveTabIndex > -1)
            return this._cachedActiveTabIndex;

        return this._activeHeaderIndex;
    },
    set_activeHeaderIndex: function(value) {
        if (!this.get_isInitialized()) {
            this._cachedActiveTabIndex = value;
        } else {
            if (value < -1 || value >= this.get_headers().length)
                throw Error.argumentOutOfRange("value");

            if (this._activeHeaderIndex != -1)
                this.get_headers()[this._activeHeaderIndex]._set_active(false);

            this._activeHeaderIndex = value;
            if (this._activeHeaderIndex != -1)
                this.get_headers()[this._activeHeaderIndex]._set_active(true);

            if (this._loaded)
                this.raiseActiveTabChanged();

            this.raisePropertyChanged("activeHeaderIndex");
        }
    },

    get_headers: function() {
        if (this._tabs == null)
            this._tabs = [];

        return this._tabs;
    },

    get_activeTab: function() {
        if (this._activeHeaderIndex > -1)
            return this.get_headers()[this._activeHeaderIndex];

        return null;
    },
    set_activeTab: function(value) {
        var i = Array.indexOf(this.get_headers(), value);
        if (i == -1)
            throw Error.argument("value", Controls.Resources.Tabs_ActiveTabArgumentOutOfRange);

        this.set_activeHeaderIndex(i);
    },

    get_autoPostBackId: function() {
        return this._autoPostBackId;
    },
    set_autoPostBackId: function(value) {
        this._autoPostBackId = value;
    },

    initialize: function() {
        Controls.SimpleTabContainer.callBaseMethod(this, "initialize");
        var elt = this.get_element();
        var header = this._header = $get(this.get_id() + "_header");
        var body = this._body = $get(this.get_id() + "_body");

        // default classes
        $common.addCssClasses(elt, ["ajax__tab_container", "ajax__tab_default"]);
        Sys.UI.DomElement.addCssClass(header, "ajax__tab_header");
        Sys.UI.DomElement.addCssClass(body, "ajax__tab_body");

        this._invalidate();
        Sys.Application.add_load(this._app_onload$delegate);
    },

    dispose: function() {
        Sys.Application.remove_load(this._app_onload$delegate);
        Controls.SimpleTabContainer.callBaseMethod(this, "dispose");
    },

    getFirstHeader: function(includeDisabled) {
        var headers = this.get_headers();
        for (var i = 0; i < headers.length; i++) {
            if (includeDisabled || headers[i].get_enabled())
                return headers[i];
        }
        return null;
    },
    getLastHeader: function(includeDisabled) {
        var headers = this.get_headers();
        for (var i = headers.length - 1; i >= 0; i--) {
            if (includeDisabled || headers[i].get_enabled())
                return headers[i];
        }
        return null;
    },
    getNextHeader: function(includeDisabled) {
        var headers = this.get_headers();
        var active = this.get_activeHeaderIndex();

        for (var i = 1; i < headers.length; i++) {
            var headerIndex = (active + i) % headers.length;
            var header = headers[headerIndex];

            if (includeDisabled || header.get_enabled())
                return header;
        }
        return null;
    },
    getPreviousHeader: function(includeDisabled) {
        var headers = this.get_headers();
        var active = this.get_activeHeaderIndex();

        for (var i = 1; i < headers.length; i++) {
            var headerIndex = (headers.length + (active - i)) % headers.length;
            var header = tabs[headerIndex];

            if (includeDisabled || header.get_enabled())
                return header;
        }
        return null;
    },
    getNearestHeader: function() {
        var prev = this.getPreviousHeader(false);
        var next = this.getNextHeader(false);

        if (prev && prev.get_headerIndex() < this._activeHeaderIndex)
            return prev;

        if (next && next.get_headerIndex() > this._activeHeaderIndex)
            return next;

        return null;
    },

    saveClientState: function() {
        var headers = this.get_headers();
        var headerState = [];

        for (var i = 0; i < headers.length; i++) {
            Array.add(headerState, headers[i].get_enabled());
        }

        var state = {
            ActiveHeaderIndex: this._activeHeaderIndex,
            HeaderState: headerState
        };

        return Sys.Serialization.JavaScriptSerializer.serialize(state);
    },

    _invalidate: function() {
        if (this.get_isInitialized()) {
            $common.removeCssClasses(this._body, ["ajax__scroll_horiz", "ajax__scroll_vert"
                                                    , "ajax__scroll_both", "ajax__scroll_auto"]);
        }
    },

    _app_onload: function(sender, e) {
        if (this._cachedActiveTabIndex != -1) {
            this.set_activeHeaderIndex(this._cachedActiveTabIndex);
            this._cachedActiveTabIndex = -1;
        }

        this._loaded = true;
    }
}
Controls.SimpleTabContainer.registerClass("Controls.SimpleTabContainer", Controls.ControlBase);

Controls.SimpleTabHeader = function(element)
{
    Controls.SimpleTabHeader.initializeBase(this, [element]);
    
    this._active = false;
    this._tab = null;
    
    this._header = element;
    this._headerOuter = null;
    this._headerInner = null;
    
    this._owner = null;
    this._enabled = true;
    this._headerIndex = -1;

    this._header_onclick$delegate = Function.createDelegate(this, this._header_onclick);
    this._header_onmouseover$delegate = Function.createDelegate(this, this._header_onmouseover);
    this._header_onmouseout$delegate = Function.createDelegate(this, this._header_onmouseout);
    this._header_onmousedown$delegate = Function.createDelegate(this, this._header_onmousedown);
    this._oncancel$delegate = Function.createDelegate(this, this._oncancel);
}

Controls.SimpleTabHeader.prototype = {
    add_click: function(handler) {
        this.get_events().addHandler("click", handler);
    },
    remove_click: function(handler) {
        this.get_events().removeHandler("click", handler);
    },
    raiseClick: function() {
        var eh = this.get_events().getHandler("click");
        if (eh)
            eh(this, Sys.EventArgs.Empty);
    },

    get_headerText: function() {
        if (this.get_isInitialized())
            return this._header.innerHTML;

        return "";
    },
    set_headerText: function(value) {
        if (!this.get_isInitialized())
            throw "PropertySetBeforeInitialization: 'headerText'";

        if (this._headerText != value) {
            this._headerTab.innerHTML = value;
            this.raisePropertyChanged("headerText");
        }
    },

    //    get_headerTab: function() {
    //        return this._header;
    //    },
    //    set_headerTab: function(value) {
    //        if (this._header != value) {
    //            if (this.get_isInitialized())
    //                throw "PropertySetAfterInitialization: 'headerTab'";

    //            this._header = value;
    //            this.raisePropertyChanged("value");
    //        }
    //    },

    get_enabled: function() {
        return this._enabled;
    },
    set_enabled: function(value) {
        if (value != this._enabled) {
            this._enabled = value;
            if (this.get_isInitialized()) {
                if (!this._enabled) {
                    this._hide();
                } else {
                    this._show();
                }
            }
            this.raisePropertyChanged("enabled");
        }
    },

    get_owner: function() {
        return this._owner;
    },
    set_owner: function(value) {
        if (this._owner != value) {
            if (this.get_isInitialized())
                throw "PropertySetAfterInitialization: 'owner'";

            this._owner = value;
            this.raisePropertyChanged("owner");
        }
    },

    get_headerIndex: function() {
        return this._headerIndex;
    },

    _get_active: function() {
        return this._active;
    },
    _set_active: function(value) {
        this._active = value;
        if (value)
            this._activate();
        else
            this._deactivate();
    },

    initialize: function() {
        Controls.SimpleTabHeader.callBaseMethod(this, "initialize");

        var owner = this.get_owner();
        if (!owner)
            throw "Cannot get owner of Header";

        this._headerIndex = owner.get_headers().length;
        Array.add(owner.get_headers(), this);

        this._headerOuterWrapper = document.createElement('span');
        this._headerInnerWrapper = document.createElement('span');

        this._tab = document.createElement('span');
        this._tab.id = this.get_id() + "_tab";
        this._header.parentNode.replaceChild(this._tab, this._header);
        this._tab.appendChild(this._headerOuterWrapper);
        this._headerOuterWrapper.appendChild(this._headerInnerWrapper);
        this._headerInnerWrapper.appendChild(this._header);

        $addHandlers(this._header, {
            click: this._header_onclick$delegate,
            mouseover: this._header_onmouseover$delegate,
            mouseout: this._header_onmouseout$delegate,
            mousedown: this._header_onmousedown$delegate,
            dragstart: this._oncancel$delegate,
            selectstart: this._oncancel$delegate,
            select: this._oncancel$delegate
        });

        Sys.UI.DomElement.addCssClass(this._headerOuterWrapper, "ajax__tab_outer");
        Sys.UI.DomElement.addCssClass(this._headerInnerWrapper, "ajax__tab_inner");
        Sys.UI.DomElement.addCssClass(this._header, "ajax__tab_tab");
        Sys.UI.DomElement.addCssClass(this.get_element(), "ajax__tab_panel");

        if (!this._enabled)
            this._hide();
    },

    dispose: function() {
        $common.removeHandlers(this._header, {
            click: this._header_onclick$delegate,
            mouseover: this._header_onmouseover$delegate,
            mouseout: this._header_onmouseout$delegate,
            mousedown: this._header_onmousedown$delegate,
            dragstart: this._oncancel$delegate,
            selectstart: this._oncancel$delegate,
            select: this._oncancel$delegate
        });
        Controls.SimpleTabHeader.callBaseMethod(this, "dispose");
    },

    _activate: function() {
        var elt = this.get_element();
        $common.setVisible(elt, true);
        Sys.UI.DomElement.addCssClass(this._tab, "ajax__tab_active");

        this._show();
        this._owner.get_element().style.visibility = 'visible';
    },
    _deactivate: function() {
        //var elt = this.get_element();
        //$common.setVisible(elt, false);
        Sys.UI.DomElement.removeCssClass(this._tab, "ajax__tab_active");
    },

    _show: function() {
        this._tab.style.display = '';
    },
    _hide: function() {
        this._tab.style.display = 'none';
        if (this._get_active()) {
            var next = this._owner.getNearestTab(false);
            if (!!next) {
                this._owner.set_activeTab(next);
            }
        }
        this._deactivate();
    },

    _header_onclick: function(e) {
        this.raiseClick();
        this.get_owner().set_activeTab(this);
    },

    _header_onmouseover: function(e) {
        Sys.UI.DomElement.addCssClass(this._tab, "ajax__tab_hover");
    },

    _header_onmouseout: function(e) {
        Sys.UI.DomElement.removeCssClass(this._tab, "ajax__tab_hover");
    },

    _header_onmousedown: function(e) {
        e.preventDefault();
    },

    _oncancel: function(e) {
        e.stopPropagation();
        e.preventDefault();
    }
}
Controls.SimpleTabHeader.registerClass("Controls.SimpleTabHeader", Sys.UI.Control);
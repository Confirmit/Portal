using System;
using System.Text;
using System.Web.UI;
using System.Xml;

namespace Controls.jQueryMenu
{
    /// <summary>
    /// The server-side control for menu. This is the server wrapper of the JavaScript dynamic menu,
    /// created on the client.
    /// </summary>
    [ToolboxData("<{0}:jQueryMenu runat=server></{0}:jQueryMenu>")]
    public class jQueryMenu : Control
    {
        private const string JQUERY_BGIFRAME_JS = "Controls.jQueryMenu.jquery.bgiframe.js";
        private const string JQUERY_DIMENSIONS_JS = "Controls.jQueryMenu.jquery.dimensions.js";
        private const string JQUERY_JDMENU_JS = "Controls.jQueryMenu.jquery.jdMenu.js";

        /// <summary>
        /// ScriptManager to process JS scripts for asynchronious calls.
        /// </summary>
        public ScriptManager ScriptManager
        {
            set { _scriptManager = value; }
        }
        private ScriptManager _scriptManager = null;

        /// <summary>
        /// Node XML element to load the menu.
        /// </summary>
        public XmlElement RootNode
        {
            get
            {
                if (xmlRootNode == null)
                {
                    if (String.IsNullOrEmpty(xmlPath))
                        return null;

                    XmlDocument xmlMenuD = new XmlDocument();
                    String mapPath = Page.MapPath("~/");
                    xmlMenuD.Load(mapPath + xmlPath);
                    XmlNodeList rootNodes = xmlMenuD.GetElementsByTagName("root");
                    xmlRootNode = rootNodes[0] as XmlElement;
                }

                return xmlRootNode;
            }
            set { xmlRootNode = value; }
        }
        private XmlElement xmlRootNode = null;

        /// <summary>
        /// Path to XML file to load the menu.
        /// </summary>
        public String XmlPath
        {
            set { xmlPath = value; }
        }
        private String xmlPath = String.Empty;

        /// <summary>
        /// <para>The name of JavaScript controller object which will process all menu's 
        /// commands on the client side.</para>
        /// <para>If this property is not set the JS functions - handlers of the 
        /// menu commands will be called directly, otherwise they will be called in the scope of
        /// this controller object. </para>
        /// </summary>
        public String ControllerObjectName
        {
            set { controllerObjectName = value; }
        }
        private String controllerObjectName = String.Empty;

        /// <summary>
        /// This method is used on the server to gain access to the JS menu object
        /// which will be created on the client and to generate the corresponding JavaScript
        /// which will process the corresponding object.
        /// </summary>
        /// <param name="id">Identifier of the menu element - the name which is passed to 
        /// </param>
        /// <returns>The name of the JS Object name which is correspond to the menu identifier.</returns>
        public String GetMenuJSObjectName(String id)
        {
            return String.Format("menu_{0}_{1}", ClientID, id);
        }

        /// <summary>
        /// This property is used on the server to gain access to the JS toolbar object
        /// which will be created on the client and to generate the corresponding JavaScript
        /// which will process the corresponding object.
        /// </summary>
        public String MenuJSObjectName
        {
            get { return String.Format("objMenu_{0}", ClientID); }
        }

        /// <summary>
        /// 
        /// </summary>
        private String menuDOMId
        {
            get { return String.Format("menu_{0}_ID", ClientID); }
        }

        /// <summary>
        /// The Java Script code which should be launched before menu creation, for example,
        /// this code could create a controller for the menu.
        /// </summary>
        /// <param name="script">The Java Script code.</param>
        public void RegisterBeforeMenuCreationScript(String script)
        {
            _beforeMenuCreationScript = script;
        }
        private String _beforeMenuCreationScript = String.Empty;

        /// <summary>
        /// The Java Script code which should be launched exactly after menu creation for it's initialization.
        /// </summary>
        /// <param name="script">The Java Script code.</param>
        public void RegisterInitMenuScript(String script)
        {
            _initMenuJSScript = script;
        }
        private String _initMenuJSScript = String.Empty;

        /// <summary>
        /// Overridden. Registers all required JS scripts for toolbar/menus creation.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (DesignMode || RootNode == null || !Visible)
                return;
            
            #region Register js scripts

            String url = Page.ClientScript.GetWebResourceUrl(GetType(), JQUERY_BGIFRAME_JS);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), JQUERY_BGIFRAME_JS))
                Page.ClientScript.RegisterClientScriptInclude(GetType(), JQUERY_BGIFRAME_JS, url);

            url = Page.ClientScript.GetWebResourceUrl(GetType(), JQUERY_DIMENSIONS_JS);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), JQUERY_DIMENSIONS_JS))
                Page.ClientScript.RegisterClientScriptInclude(GetType(), JQUERY_DIMENSIONS_JS, url);

            url = Page.ClientScript.GetWebResourceUrl(GetType(), JQUERY_JDMENU_JS);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), JQUERY_JDMENU_JS))
                Page.ClientScript.RegisterClientScriptInclude(GetType(), JQUERY_JDMENU_JS, url);

            #endregion

            if (_scriptManager == null || !_scriptManager.IsInAsyncPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(),
                                  "ExecuteDestroyMenu" + ClientID,
                                  destroyMenuJSScript,
                                  true);

                if (!String.IsNullOrEmpty(_beforeMenuCreationScript))
                {
                    Page.ClientScript.RegisterStartupScript(GetType()
                        , "ExecuteBeforeMenuCreationOnReady" + ClientID
                        , _beforeMenuCreationScript
                        , true);
                }

                Page.ClientScript.RegisterStartupScript(GetType()
                       , "ExecuteCreateMenuFunction" + ClientID
                       , createMenuFunction
                       , true);

                Page.ClientScript.RegisterStartupScript(GetType()
                    , "ExecuteMenuCreateOnReady" + ClientID
                    , string.Format("Sys.Application.add_init(function () {{ {0} }});", string.Format("{0}.call();", createMenuFunctionName))
                    , true);

                if (!String.IsNullOrEmpty(_initMenuJSScript))
                {
                    Page.ClientScript.RegisterStartupScript(GetType()
                        , "ExecuteMenuInitOnReady" + ClientID
                        , _initMenuJSScript
                        , true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType()
                    , "ExecuteDestroyMenu" + ClientID
                    , destroyMenuJSScript
                    , true);

                if (!String.IsNullOrEmpty(_beforeMenuCreationScript))
                {
                    ScriptManager.RegisterStartupScript(this, GetType()
                        , "ExecuteBeforeMenuCreationAsyncCall" + ClientID
                        , _beforeMenuCreationScript
                        , true);
                }

                ScriptManager.RegisterStartupScript(this, GetType()
                    , "ExecuteToolbarCreateOnAsyncCall" + ClientID
                    , String.Format("{0}.call();", createMenuFunctionName)
                    , true);

                if (!String.IsNullOrEmpty(_initMenuJSScript))
                    ScriptManager.RegisterStartupScript(this, GetType()
                        , "ExecuteMenuInitOnAsyncCall" + ClientID
                        , _initMenuJSScript
                        , true);
            }            
        }

        private String createMenuFunctionName
        {
            get { return String.Format("create_{0}", MenuJSObjectName); }
        }

        private String createMenuFunction
        {
            get
            {
                StringBuilder script = new StringBuilder();
                script.AppendFormat(" var {1} = null; var {0} = function () {{ ", createMenuFunctionName, MenuJSObjectName);
                //script.Append(" $(function(){ ");
                script.AppendFormat(" {0} = $('#{1}').jdMenu(); "
                                    , MenuJSObjectName
                                    , menuDOMId);
                //script.Append(" }); ");
                script.Append(" }; ");
                
                return script.ToString();
            }
        }

        private String destroyMenuJSScript
        {
            get { return string.Format(" if (typeof({0}) != 'undefined' && {0} != null) {0}.destroy();", MenuJSObjectName); }
        }

        #region [ Rendering ]

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode || RootNode == null || !Visible)
                return;

            base.Render(writer);

            writer.Write("<div id=\"container_{0}\" style=\"width:100%\" ", ClientID);
            if (RootNode.Attributes["align"] != null)
            {
                writer.Write("style=\"text-align:{0};\"", RootNode.Attributes["align"].InnerText);
            }
            writer.Write(">{0}</div>", createMenuJSScript);
        }

        private string createMenuJSScript
        {
            get
            {
                StringBuilder script = new StringBuilder();
                script.AppendFormat("<ul id='{0}' class='jd_menu jd_menu_slate'>", menuDOMId);

                foreach (XmlNode item in RootNode.ChildNodes)
                {
                    if (item.Name != "node")
                        continue;

                    addMenuItem(script, item);
                }

                script.Append("</ul>");
                return script.ToString();
            }
        }

        private void addMenuItem(StringBuilder script, XmlNode item)
        {
            string name = item.Attributes["name"] != null
                                          ? item.Attributes["name"].Value
                                          : string.Empty;

            string id = item.Attributes["id"] != null
                              ? item.Attributes["id"].Value
                              : string.Empty;

            string action = item.Attributes["action"] != null
                                ? string.IsNullOrEmpty(controllerObjectName)
                                      ? item.Attributes["action"].Value
                                      : string.Format("{0}.{1}", controllerObjectName, item.Attributes["action"].Value)
                                : string.Empty;

            string img = item.Attributes["img"] != null
                             ? string.Format("<img src=\"{0}\" />", item.Attributes["img"].Value)
                             : string.Empty;

            string value = string.IsNullOrEmpty(img)
                               ? name
                               : string.Format(
                                     "<div style=\"float: left;\">{0}</div><div style=\"float: left;\">{1}</div>"
                                     , img, name);

            if (!item.HasChildNodes)
                script.AppendFormat(" <li id=\"{2}\" onclick=\"{1}\">{0}</li>"
                                    , value , action , id);
            else
            {
                script.AppendFormat(" <li id=\"{2}\" onclick=\"{1}\">{0}<ul>"
                                    , value, action, id);

                foreach (XmlNode node in item.ChildNodes)
                {
                    addMenuItem(script, node);
                }

                script.Append("</ul></li>");
            }
        }

        #endregion
    }
}
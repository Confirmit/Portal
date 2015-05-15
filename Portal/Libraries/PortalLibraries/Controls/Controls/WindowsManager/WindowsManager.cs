using System;
using System.Text;
using System.Web.UI;

namespace Controls.WindowsManager
{
    /// <summary>
    /// The server-side wrapper control for pop up Windows support. 
    /// </summary>
    [ToolboxData("<{0}:WindowsManager runat=server></{0}:WindowsManager>")]
    public class WindowsManager : Control
    {
        /// <summary>
        /// The name of windows manager client side JS object.
        /// </summary>
        public String ClientJSObjectName
        {
            get
            {
                return String.Format("{0}_winManager", ClientID);
            }
        }

        /// <summary>
        /// ScriptManager to process JS scripts for asynchronous calls.
        /// </summary>
        public ScriptManager ScriptManager
        {
            set { _scriptManager = value; }
        }
        private ScriptManager _scriptManager = null;

        /// <summary>
        /// Register the JS script for opening of the modal dialog pop up window.
        /// </summary>
        /// <param name="sourceUrl">The URL of the source view to display in the dialog.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="width">Dialog width.</param>
        /// <param name="height">Dialog height.</param>
        public void RegisterStaticDoModalScript(String sourceUrl, String title, Int32 width, Int32 height)
        {
            StringBuilder script = new StringBuilder();

            script.AppendFormat(" var {0} = {1}.createWindow(); "
                , clientStaticWindowName, ClientJSObjectName);
            script.AppendFormat(" {0}.setTitle('{1}'); "
                , clientStaticWindowName, title);
            script.AppendFormat(" {0}.setSize({1}, {2}); "
                , clientStaticWindowName, width, height);

            script.AppendFormat(" {0}.doModal(); "
                , clientStaticWindowName);

            script.AppendFormat(" {0}.loadWindow({1}, '{2}'); "
                , ClientJSObjectName, clientStaticWindowName, sourceUrl);

            _registerStaticDoModalScript = script.ToString();
        }
        private String _registerStaticDoModalScript = String.Empty;

        private String clientStaticWindowName
        {
            get { return String.Format("{0}_staticWindow", ClientID); }
        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
 	        base.OnPreRender(e);

            if (DesignMode || !Visible)
                return;

            String url = Page.ClientScript.GetWebResourceUrl(GetType(), WINDOW_JS);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), WINDOW_JS))
                Page.ClientScript.RegisterClientScriptInclude(GetType(), WINDOW_JS, url);

            url = Page.ClientScript.GetWebResourceUrl(GetType(), WINDOWS_MANAGER_JS);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), WINDOWS_MANAGER_JS))
                Page.ClientScript.RegisterClientScriptInclude(GetType(), WINDOWS_MANAGER_JS, url);

            if (_scriptManager == null || !_scriptManager.IsInAsyncPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType()
                    , "ExecuteWindowsManagerCreateOnReady" + ClientID
                    , String.Format(@"
                        var {0} = null;
                        Sys.Application.add_init(function(){{ {1} }});"
                            , ClientJSObjectName, createWindowsManagerJSScript)
                    , true);

                if (!String.IsNullOrEmpty(_registerStaticDoModalScript))
                {
                    Page.ClientScript.RegisterStartupScript(GetType()
                        , "DoModalDialogOnInit" + ClientID
                        , String.Format("Sys.Application.add_init(function(){{ {0} }});"
                            , _registerStaticDoModalScript)
                        , true);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(_registerStaticDoModalScript))
                {
                    ScriptManager.RegisterStartupScript(this.Page, GetType()
                        , "DoModalDialogOnAsyncCall" + ClientID
                        , _registerStaticDoModalScript
                        , true);
                }
            }                    
        }

        private String createWindowsManagerJSScript
        {
            get
            {
                return String.Format(
                    " {0} = new IEMS.WebForms.WindowsManager(); ", ClientJSObjectName
                );
            }
        }

        private const string WINDOWS_MANAGER_JS = "Controls.WindowsManager.WindowsManager.js";
        private const string WINDOW_JS = "Controls.WindowsManager.Window.js";
    }
}

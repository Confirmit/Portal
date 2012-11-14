using System;
using System.Web.UI;

namespace Controls.WebGenerator
{
    public abstract class MenuBase : Control, INamingContainer
    {
        #region Web Form Designer generated code

        /// <summary>
        /// Executes on page initialization.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        #region Overrides

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (DesignMode)
                return;

            if (!(Visible))
                return;

            if (!(Page.ClientScript.IsClientScriptIncludeRegistered("Menu_vmenu")))
                Page.ClientScript.RegisterClientScriptInclude("Menu_vmenu",
                                                              Page.ClientScript.GetWebResourceUrl(GetType(),
                                                                                                  "Controls.WebGenerator.vmenu.js"));
        }

        /// <summary>
        /// Renders the control and writes HTML string into it.
        /// </summary>
        /// <param name="writer">The <see cref="System.Web.UI.HtmlTextWriter"/> to write control into.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
                return;
            if (!(Visible))
                return;

            RenderMenu(writer);
        }

        protected abstract void RenderMenu(HtmlTextWriter writer);

        #endregion

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
        protected String controllerObjectName = String.Empty;
    }
}
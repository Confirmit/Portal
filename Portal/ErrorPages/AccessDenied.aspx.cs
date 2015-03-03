using System;
using System.Linq;
using System.Web.UI.WebControls;
using Portal.Helpers;

public partial class ErrorPages_AccessDenied : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var finder = new ControlFinder<Menu>();
        finder.FindControls(this);

        var menu = finder.FoundControls.First();
        menu.Enabled = false;

        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["aspxerrorpath"]))
        {
            lblErrorDescription.Text = 
                string.Format(lblErrorDescription.Text, this.Page.Request.QueryString["aspxerrorpath"]);
        }
        else
        {
            lblErrorDescription.Text =
                string.Format(lblErrorDescription.Text, string.Empty);
        }
    }
}

 

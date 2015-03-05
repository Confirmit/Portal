using System;

public partial class ErrorPages_AccessDenied : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

 

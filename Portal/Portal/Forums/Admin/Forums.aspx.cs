using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetForums;
using AspNetForums.Components;

public partial class Forums_Admin_Forums: BaseWebPage
{
    public void Page_Load(Object sender, EventArgs e)
    {
        // Is this user isn't an administrator?
        if (!Users.GetLoggedOnUser().IsAdministrator)
            Context.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.UnableToAdminister));
    }
}

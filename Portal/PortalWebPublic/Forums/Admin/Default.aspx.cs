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

public partial class Forums_Admin_Default: BaseWebPage
{
    public string imagePathForAdminIcon;
    public string imagePathForUserIcon;

    public void Page_Load(Object sender, EventArgs e)
    {
        // Is this user isn't an administrator?
        if (!Users.GetLoggedOnUser().IsAdministrator)
            Context.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.UnableToAdminister));

        // Build Image path based on Skin
        imagePathForAdminIcon = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + Globals.Skin + "/images/administration_icon.gif";
        imagePathForUserIcon = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + Globals.Skin + "/images/users_icon.gif";
    }

}

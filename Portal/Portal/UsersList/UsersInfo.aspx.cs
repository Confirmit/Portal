using System;

// TODO: добавить проверку на режим отображения страицы - админисративный или пользовательский видимость контрола adminMenu)

public partial class UsersInfo : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var userIDStr = Request.QueryString["UserID"];
        if (string.IsNullOrEmpty(userIDStr))
            Response.Redirect(hlMain.NavigateUrl);

        int userID;
        if (!Int32.TryParse(userIDStr, out userID))
            Response.Redirect(hlMain.NavigateUrl);

        if(CurrentUser.ID != userID && !CurrentUser.IsInRole("Administrator"))
            Response.Redirect(hlMain.NavigateUrl);

        userInfoView.UserID = userID;
    }
}

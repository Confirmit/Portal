using System;

// TODO: добавить проверку на режим отображения страицы - админисративный или пользовательский видимость контрола adminMenu)

public partial class UsersInfo : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Установка режима работы.
        //if (!Page.CurrentUser.IsInRole("Administrator"))
        //{ ControlMode = Mode.Standard; }

        if (IsPostBack)
            return;

        // Получить ID пользователя, информация которого отображается.
        string userIDStr = Request.QueryString["UserID"];
        if (string.IsNullOrEmpty(userIDStr))
            Response.Redirect(hlMain.NavigateUrl);

        int userID = 0;
        if (!Int32.TryParse(userIDStr, out userID))
            Response.Redirect(hlMain.NavigateUrl);

        userInfoView.UserID = userID;
    }
}

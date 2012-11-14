using System;

// TODO: �������� �������� �� ����� ����������� ������� - ��������������� ��� ���������������� ��������� �������� adminMenu)

public partial class UsersInfo : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // ��������� ������ ������.
        //if (!Page.CurrentUser.IsInRole("Administrator"))
        //{ ControlMode = Mode.Standard; }

        if (IsPostBack)
            return;

        // �������� ID ������������, ���������� �������� ������������.
        string userIDStr = Request.QueryString["UserID"];
        if (string.IsNullOrEmpty(userIDStr))
            Response.Redirect(hlMain.NavigateUrl);

        int userID = 0;
        if (!Int32.TryParse(userIDStr, out userID))
            Response.Redirect(hlMain.NavigateUrl);

        userInfoView.UserID = userID;
    }
}

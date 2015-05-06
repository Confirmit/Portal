using System;

using UlterSystems.PortalLib.BusinessObjects;

public partial class Admin_AdminUsersPage : BaseWebPage
{
    #region PageState enum
    
    /// <summary>
    /// Состояние страницы.
    /// </summary>
    private enum PageState
    {
        Normal,
        Creating
    }

    #endregion

    #region Properties

    /// <summary>
    /// Состояние страницы.
    /// </summary>
    private PageState State
    {
        get
        {
            return ViewState["State"] == null
                       ? PageState.Normal
                       : (PageState) ViewState["State"];
        }
        set
        {
            ViewState["State"] = (int)value;
            // Установить видимость контрола.
            plhUsersList.Visible = (value == PageState.Normal);
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        userGrid.FilterControl = usersFilter;
        userGrid.UserChanging += userInfo.OnUserChanging;

        if (IsPostBack)
            return;

        State = PageState.Normal;
        userInfo.UserID = CurrentUser.ID;
    }

    /// <summary>
    /// Переход в режим создания нового пользователя.
    /// </summary>
    protected void createNewUser(object sender, EventArgs e)
    {
        State = PageState.Creating;
        
        userInfo.UserID = null;
        userGrid.SelectedIndex = -1;
    }

    /// <summary>
    /// Сохраниение информации о пользователе.
    /// </summary>
    protected void applyUserInfo(object sender, EventArgs e)
    {
        Person updatedUser = userInfo.SaveUserChanges();
        if (State == PageState.Creating)
            State = PageState.Normal;

        try
        {
            userInfo.UserID = updatedUser.ID;
        }
        catch
        {
            userInfo.UserID = CurrentUser.ID;
        }

        userGrid.GridDataBind();
        //userGrid.SelectedUserID = userInfo.UserID.Value;
    }

    /// <summary>
    /// Отменяет изменения информации о пользователе.
    /// </summary>
    protected void cancelUserInfo(object sender, EventArgs e)
    {
        if (State == PageState.Creating)
            State = PageState.Normal;
    }
}

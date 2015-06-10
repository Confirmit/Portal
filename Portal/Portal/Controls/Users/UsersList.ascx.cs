using System;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using Core;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Controls_UsersList : BaseUserControl
{
    public DateTime Date
    {
        get
        {
            return ViewState["Date"] == null
                       ? DateTime.Today
                       : (DateTime)ViewState["Date"];
        }
        set
        {
            ViewState["Date"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridUsersList.DataSource = UserList.GetStatusesList(Date, isDescendingSortDirection: true,
                propertyName: "LastName");
            GridUsersList.DataBind();
            ViewState["CurrentGridViewSortEventSerializableArgs"] = new GridViewSortEventSerializableArgs("LastName",
                SortDirection.Ascending);
            GridUsersList.Columns[1].HeaderStyle.CssClass = "AscSorting";
        }

        if (!Page.CurrentUser.IsInRole(RolesEnum.Administrator))
            GridUsersList.Columns[3].Visible = false;
    }

    /// <summary>
    /// Ïðèâÿçêà äàííûõ ïîëüçîâàòåëåé ê ýëåìåíòàì óïðàâëåíèÿ.
    /// </summary>
    protected void OnUserInfoBound(object sender, GridViewRowEventArgs gridViewRowEventArgs)
    {
        var rowDataItem = gridViewRowEventArgs.Row.DataItem;
        if (!(rowDataItem is UserStatusInfo))
            return;

        var userStatusInfo = (UserStatusInfo)rowDataItem;

        if (Page.CurrentUser.ID.HasValue && userStatusInfo.UserID == Page.CurrentUser.ID.Value)
            gridViewRowEventArgs.Row.Style.Add("background-color", "#99e6ff");

        var imageButton = (ImageButton)gridViewRowEventArgs.Row.FindControl("btnIll");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString();

        imageButton = (ImageButton)gridViewRowEventArgs.Row.FindControl("btnTrustIll");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString();

        imageButton = (ImageButton)gridViewRowEventArgs.Row.FindControl("btnBusinessTrip");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString();

        imageButton = (ImageButton)gridViewRowEventArgs.Row.FindControl("btnVacation");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString();

        imageButton = (ImageButton)gridViewRowEventArgs.Row.FindControl("btnLesson");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString();
    }


    protected void SortingCommand_Click(object sender, GridViewSortEventArgs e)
    {
        var oldGridViewSortEventArgs = (GridViewSortEventSerializableArgs)ViewState["CurrentGridViewSortEventSerializableArgs"];
        SortDirection newSortDirection;
        if (oldGridViewSortEventArgs.SortExpression == e.SortExpression)
        {
            if (oldGridViewSortEventArgs.CurrentDirection == SortDirection.Ascending)
                newSortDirection = SortDirection.Descending;
            else
                newSortDirection = SortDirection.Ascending;
        }
        else
            newSortDirection = SortDirection.Ascending;
        ViewState["CurrentGridViewSortEventSerializableArgs"] = new GridViewSortEventSerializableArgs(e.SortExpression, newSortDirection);

        var isDescendingSortDirection = newSortDirection == SortDirection.Ascending;
        var sortedUsers = UserList.GetStatusesList(Date, isDescendingSortDirection, e.SortExpression);
        GridUsersList.DataSource = sortedUsers;
        GridUsersList.DataBind();

        foreach (DataControlField column in GridUsersList.Columns)
        {
            if (column.SortExpression == e.SortExpression)
            {
                if (isDescendingSortDirection)
                    column.HeaderStyle.CssClass = "AscSorting";
                else
                    column.HeaderStyle.CssClass = "DescSorting";
            }
            else
                column.HeaderStyle.CssClass = "";
        }
    }

    protected virtual void OnSetIll(object sender, EventArgs e)
    {
        CreateAbsenceEvent(sender, WorkEventType.Ill);
    }

    protected virtual void OnSetTrustIll(object sender, EventArgs e)
    {
        CreateAbsenceEvent(sender, WorkEventType.TrustIll);
    }

    protected virtual void OnSetBusinessTrip(object sender, EventArgs e)
    {
        CreateAbsenceEvent(sender, WorkEventType.BusinessTrip);
    }

    protected virtual void OnSetVacation(object sender, EventArgs e)
    {
        CreateAbsenceEvent(sender, WorkEventType.Vacation);
    }

    protected void OnSetLesson(object sender, EventArgs e)
    {
        try
        {
            if (!(sender is ImageButton))
                return;

            var imageButton = (ImageButton)sender;

            int userID;
            if (!Int32.TryParse(imageButton.CommandArgument, out userID))
                return;

            var userEvents = new UserWorkEvents(userID);
            var duration = TimeSpan.FromMinutes(45);
            userEvents.AddLatestClosedWorkEvent(duration, WorkEventType.TimeOff);

            RefreshTable();
        }
        catch (Exception ex)
        {
            lblException.Text = ex.Message;
            lblException.Visible = true;
        }
    }

    private void CreateAbsenceEvent(Object sender, WorkEventType eventType)
    {
        try
        {
            if (!(sender is ImageButton))
                return;

            var imageButton = (ImageButton)sender;

            int userID;
            if (!Int32.TryParse(imageButton.CommandArgument, out userID))
                return;

            var userEvents = new UserWorkEvents(userID);
            userEvents.CreateAbsenceEvent(eventType, Date);

            RefreshTable();
        }
        catch (Exception ex)
        {
            lblException.Text = ex.Message;
            lblException.Visible = true;
        }
    }

    private void RefreshTable()
    {
        var gridViewSortEventArgs = (GridViewSortEventSerializableArgs)ViewState["CurrentGridViewSortEventSerializableArgs"];
        var sortedUsers = UserList.GetStatusesList(Date,
            gridViewSortEventArgs.CurrentDirection == SortDirection.Ascending, gridViewSortEventArgs.SortExpression);
        GridUsersList.DataSource = sortedUsers;
        GridUsersList.DataBind();
    }
}
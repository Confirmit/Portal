using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using Core;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Controls_UsersList : BaseUserControl
{
    #region Свойства

    /// <summary>
    /// Дата, за которую отображаются события.
    /// </summary>
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

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.CurrentUser != null)
        {
            if (Page.CurrentUser.IsInRole(RolesEnum.Administrator))
                GridUsersList.Columns[3].Visible = true;
            else
                GridUsersList.Columns[3].Visible = false;
        }

        if (!IsPostBack)
        {
            GridUsersList.DataSource = UserList.GetStatusesList(Date, isDescendingSortDirection: true, propertyName: "LastName");
            GridUsersList.DataBind();
            ViewState["CurrentGridViewSortEventSerializableArgs"] = new GridViewSortEventSerializableArgs("LastName", SortDirection.Ascending);
        }
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

    private void RefreshTable()
    {
        var gridViewSortEventArgs = (GridViewSortEventSerializableArgs)ViewState["CurrentGridViewSortEventSerializableArgs"];
        var isDescendingSortDirection = gridViewSortEventArgs.CurrentDirection == SortDirection.Ascending;
        var sortedUsers = UserList.GetStatusesList(Date, isDescendingSortDirection, gridViewSortEventArgs.SortExpression);
        GridUsersList.DataSource = sortedUsers;
        GridUsersList.DataBind();
    }

    protected void OnRowDataBoundHandler(object sender, GridViewRowEventArgs gridViewRowEventArgs)
    {
        var row = gridViewRowEventArgs.Row;
        var dataItem = gridViewRowEventArgs.Row.DataItem;
        if (!(dataItem is UserStatusInfo))
            return;

        var userStatusInfo = (UserStatusInfo)dataItem;

        if (Page.CurrentUser != null && (Page.CurrentUser.ID.HasValue && userStatusInfo.UserID == Page.CurrentUser.ID.Value))
            row.CssClass = "gridview-selectedrow";

        var imageButton = (ImageButton)row.FindControl("IllImageButton");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString(CultureInfo.InvariantCulture);

        imageButton = (ImageButton)row.FindControl("TrustIllImageButton");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString(CultureInfo.InvariantCulture);

        imageButton = (ImageButton)row.FindControl("BusinessTripImageButton");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString(CultureInfo.InvariantCulture);

        imageButton = (ImageButton)row.FindControl("VacationImageButton");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString(CultureInfo.InvariantCulture);

        imageButton = (ImageButton)row.FindControl("LessonImageButton");
        if (imageButton != null)
            imageButton.CommandArgument = userStatusInfo.UserID.ToString(CultureInfo.InvariantCulture);
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

            int userId;
            if (!Int32.TryParse(imageButton.CommandArgument, out userId))
                return;

            var userEvents = new UserWorkEvents(userId);

            var duration = TimeSpan.FromMinutes(45);
            userEvents.AddLatestClosedWorkEvent(duration, WorkEventType.TimeOff);

            RefreshTable();
        }
        catch (Exception ex)
        {
            ExceptionLabel.Text = ex.Message;
            ExceptionLabel.Visible = true;
        }
    }

    /// <summary>
    /// Create absence event for user.
    /// </summary>
    /// <param name="sender">Image button.</param>
    /// <param name="eventType">Type of event.</param>
    private void CreateAbsenceEvent(Object sender, WorkEventType eventType)
    {
        try
        {
            if (!(sender is ImageButton))
                return;

            var imageButton = (ImageButton)sender;

            int userId;
            if (!Int32.TryParse(imageButton.CommandArgument, out userId))
                return;

            var userEvents = new UserWorkEvents(userId);
            userEvents.CreateAbsenceEvent(eventType, Date);

            RefreshTable();
        }
        catch (Exception ex)
        {
            ExceptionLabel.Text = ex.Message;
            ExceptionLabel.Visible = true;
        }
    }
}
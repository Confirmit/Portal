using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using ConfirmIt.PortalLib.DAL;
using UlterSystems.PortalLib.BusinessObjects;
using Controls;

public partial class ObjectManagerControl : BaseUserControl
{
    #region  [ Page events ]

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        simpleTabContainer.ActiveTabChanged += OnObjectTypeChanged;
        ddlUsers.SelectedIndexChanged += OnChangeUser;

        if (IsPostBack)
            return;

        bindData();
    }

    #endregion

    #region [ Binding ]

    private void bindData()
    {
        bindUsers();
    }

    

    private void bindUsers()
    {
        List<ListItem> usersList = new List<ListItem>();
        usersList.Add(new ListItem(""));
        usersList.Add(new ListItem((string)GetLocalResourceObject("Office.Text"), "0"));

        var yarUserList = UserList.GetYaroslavlOfficeUsersList().OrderBy(user => user.FullName);
        foreach (var user in yarUserList)
        {
            usersList.Add(new ListItem(user.FullName, user.ID.ToString()));
        }

        ddlUsers.Items.AddRange(usersList.ToArray());
        ddlGrantTo.Items.AddRange(usersList.ToArray());
    }

    #endregion

    private void OnObjectTypeChanged(object sender, int headerIndex)
    {
        ddlObjects.Items.Clear();

        lblHolderName.Text = lblOwnerName.Text = string.Empty;
        objectHistoryGrid.Visible = false;
        divTake.Visible = divGrant.Visible = false;
    }

    

    private void OnChangeUser(object sender, EventArgs e)
    {
        objectsOnHandsGrid.Visible = !string.IsNullOrEmpty(ddlUsers.SelectedValue);
        if (string.IsNullOrEmpty(ddlUsers.SelectedValue))
            return;

        objectsOnHandsGrid.DataBind();
    }

    
}
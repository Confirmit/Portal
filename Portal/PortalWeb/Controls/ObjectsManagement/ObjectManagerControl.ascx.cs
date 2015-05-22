using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using ConfirmIt.PortalLib.DAL;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using UlterSystems.PortalLib.BusinessObjects;
using Controls;

public partial class ObjectManagerControl : BaseUserControl
{
    #region  [ Page events ]

    protected override void OnInit(EventArgs e)
    {
        bindObjectTypes();
        base.OnInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        simpleTabContainer.ActiveTabChanged += OnObjectTypeChanged;
        ddlObjects.SelectedIndexChanged += OnSelectedObject;
        ddlUsers.SelectedIndexChanged += OnChangeUser;

        btnTake.Click += OnTakeObject;
        btnGrant.Click += OnGrantObject;

        if (IsPostBack)
            return;

        bindData();
    }

    #endregion

    #region [ Binding ]

    private void bindData()
    {
        bindObjects();
        bindUsers();
    }

    private void bindObjectTypes()
    {
        foreach (var value in Enum.GetValues(typeof(RequestObjectType.ObjectType)))
        {
            simpleTabContainer.Headers.Add(new SimpleTabHeader { HeaderText = Enum.GetName(typeof(RequestObjectType.ObjectType), value) });
        }
    }

    private void bindObjects()
    {
        int index = simpleTabContainer.ActiveHeaderIndex;
        var typeValue = (RequestObjectType.ObjectType)Enum.Parse(typeof(RequestObjectType.ObjectType), index.ToString());

        ddlObjects.Items.Add(new ListItem(" "));
        foreach (RequestObject entity in RequestObject.GetAllRequestObjects(typeValue))
        {
            ddlObjects.Items.Add(new ListItem(entity.Title, entity.ID.ToString()));
        }
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
        bindObjects();

        lblHolderName.Text = lblOwnerName.Text = string.Empty;
        objectHistoryGrid.Visible = false;
        divTake.Visible = divGrant.Visible = false;
    }

    private void OnSelectedObject(object sender, EventArgs e)
    {
        if (ddlObjects.SelectedIndex == 0)
        {
            divTake.Visible = divGrant.Visible = false;
            lblOwnerName.Text = lblHolderName.Text = ddlGrantTo.SelectedValue = string.Empty;
            objectHistoryGrid.Visible = false;

            return;
        }

        int objId = int.Parse(ddlObjects.SelectedValue);
        objectHistoryGrid.DataBind(objId);
        objectHistoryGrid.Visible = true;

        int index = simpleTabContainer.ActiveHeaderIndex;
        int? ownerId;
        lblOwnerName.Text = SiteProvider.RequestObjects.GetOwnerName(objId, out ownerId);

        int? holderId;
        lblHolderName.Text = SiteProvider.RequestObjects.GetHolderName(objId, out holderId);

        if (holderId != null && holderId == ((Person)CurrentUser).ID)
        {
            divTake.Visible = false;
            divGrant.Visible = true;
            ddlGrantTo.SelectedIndex = 0;
        }
        else
        {
            divTake.Visible = true;
            divGrant.Visible = false;
        }
    }

    private void OnChangeUser(object sender, EventArgs e)
    {
        objectsOnHandsGrid.Visible = !string.IsNullOrEmpty(ddlUsers.SelectedValue);
        if (string.IsNullOrEmpty(ddlUsers.SelectedValue))
            return;

        objectsOnHandsGrid.DataBind();
    }

    #region Buttons events

    private void OnTakeObject(Object sender, EventArgs e)
    {
        ((Person)CurrentUser).TakeObject(int.Parse(ddlObjects.SelectedValue));
        objectsOnHandsGrid.DataBind();

        OnSelectedObject(null, null);
    }

    private void OnGrantObject(Object sender, EventArgs e)
    {
        int? userId;
        if (ddlGrantTo.SelectedIndex == 1)
            userId = null;
        else
            userId = int.Parse(ddlGrantTo.SelectedValue);

        ((Person)CurrentUser).GrantObject(int.Parse(ddlObjects.SelectedValue), userId);
        objectsOnHandsGrid.DataBind();

        OnSelectedObject(null, null);
    }

    #endregion
}
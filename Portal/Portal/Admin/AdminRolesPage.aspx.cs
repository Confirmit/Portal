using System;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;

/// <summary>
/// Page for creating, deleting, updating roles information
/// </summary>
public partial class Admin_AdminRolesPage : BaseWebPage
{
	protected void Page_Load( object sender, EventArgs e )
	{
		lblErrorDescription.Text = string.Empty;
	}

	#region Event handlers for grid view

    protected void gvRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        dvSelectedRole.ChangeMode(DetailsViewMode.Edit);
    }

    protected void gvRoles_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btn = e.Row.Cells[e.Row.Cells.Count - 1].Controls[0] as ImageButton;
            if (btn != null)
            {
                btn.OnClientClick = string.Format("if (confirm('{0}') == false) return false; ",
                                                  GetLocalResourceObject("ConfirmDeleteMessage"));
            }
        }
    }

    protected void gvRoles_RowDeleted( object sender, GridViewDeletedEventArgs e )
	{
		gvRoles.SelectedIndex = -1;
		gvRoles.DataBind();
		dvSelectedRole.ChangeMode( DetailsViewMode.Insert );
	}

	#endregion

	#region Event handlers for details view

	protected void dvSelectedRole_ItemInserted( object sender, DetailsViewInsertedEventArgs e )
	{
		gvRoles.SelectedIndex = -1;
		gvRoles.DataBind();
	}

	protected void dvSelectedRole_ItemCommand( object sender, DetailsViewCommandEventArgs e )
	{
		if( e.CommandName == "Cancel" )
		{
			gvRoles.SelectedIndex = -1;
			gvRoles.DataBind();
		}
	}

    protected void dvSelectedRole_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        TextBox tbRoleID = dvSelectedRole.FindControl("tbRoleID") as TextBox;
        if (tbRoleID != null)
        {
            if (string.IsNullOrEmpty(tbRoleID.Text))
            {
                e.Cancel = true;
                lblErrorDescription.Text = (string) this.GetLocalResourceObject("NoRoleID");
                return;
            }

            foreach (Role role in Role.GetAllRoles())
            {
                if (string.Compare(tbRoleID.Text, role.RoleID) == 0)
                {
                    e.Cancel = true;
                    lblErrorDescription.Text = string.Format((string) this.GetLocalResourceObject("RoleExists"),
                                                             tbRoleID.Text);
                    return;
                }
            }
        }

        Controls_MLTextBox mltbRoleName = dvSelectedRole.FindControl("mltbRoleName") as Controls_MLTextBox;
        if (mltbRoleName != null)
        {
            e.Values["Name"] = mltbRoleName.MultilingualText;
            if (string.IsNullOrEmpty(mltbRoleName.MultilingualText.ToString()))
            {
                e.Cancel = true;
                lblErrorDescription.Text = (string) this.GetLocalResourceObject("NoRoleName");
                return;
            }
        }

        Controls_MLTextBox mltbRoleDescription = dvSelectedRole.FindControl("mltbRoleDescription") as Controls_MLTextBox;
        if (mltbRoleDescription != null)
        {
            e.Values["Description"] = mltbRoleDescription.MultilingualText;
            if (string.IsNullOrEmpty(mltbRoleDescription.MultilingualText.ToString()))
            {
                e.Cancel = true;
                lblErrorDescription.Text = (string) this.GetLocalResourceObject("NoRoleDescription");
                return;
            }
        }
    }

    protected void dvSelectedRole_DataBound(object sender, EventArgs e)
    {
        Role role = dvSelectedRole.DataItem as Role;
        if (role == null)
            return;

        TextBox tbRoleID = dvSelectedRole.FindControl("tbRoleID") as TextBox;
        if (tbRoleID != null)
            tbRoleID.Enabled = false;

        Controls_MLTextBox mltbRoleName = dvSelectedRole.FindControl("mltbRoleName") as Controls_MLTextBox;
        if (mltbRoleName != null)
            mltbRoleName.MultilingualText = role.Name;

        Controls_MLTextBox mltbRoleDescription = dvSelectedRole.FindControl("mltbRoleDescription") as Controls_MLTextBox;
        if (mltbRoleDescription != null)
            mltbRoleDescription.MultilingualText = role.Description;
    }

    protected void dvSelectedRole_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        int id = (int) dvSelectedRole.SelectedValue;

        TextBox tbRoleID = dvSelectedRole.FindControl("tbRoleID") as TextBox;
        if (tbRoleID != null)
        {
            if (string.IsNullOrEmpty(tbRoleID.Text))
            {
                e.Cancel = true;
                lblErrorDescription.Text = (string) this.GetLocalResourceObject("NoRoleID");
                return;
            }

            foreach (Role role in Role.GetAllRoles())
            {
                if (role.ID == id)
                    continue;

                if (string.Compare(tbRoleID.Text, role.RoleID) == 0)
                {
                    e.Cancel = true;
                    lblErrorDescription.Text = string.Format((string) this.GetLocalResourceObject("RoleExists"),
                                                             tbRoleID.Text);
                    return;
                }
            }
        }

        Controls_MLTextBox mltbRoleName = dvSelectedRole.FindControl("mltbRoleName") as Controls_MLTextBox;
        if (mltbRoleName != null)
        {
            e.NewValues["Name"] = mltbRoleName.MultilingualText;
            if (string.IsNullOrEmpty(mltbRoleName.MultilingualText.ToString()))
            {
                e.Cancel = true;
                lblErrorDescription.Text = (string) this.GetLocalResourceObject("NoRoleName");
                return;
            }
        }

        Controls_MLTextBox mltbRoleDescription = dvSelectedRole.FindControl("mltbRoleDescription") as Controls_MLTextBox;
        if (mltbRoleDescription != null)
        {
            e.NewValues["Description"] = mltbRoleDescription.MultilingualText;
            if (string.IsNullOrEmpty(mltbRoleDescription.MultilingualText.ToString()))
            {
                e.Cancel = true;
                lblErrorDescription.Text = (string) this.GetLocalResourceObject("NoRoleDescription");
                return;
            }
        }
    }

    protected void dvSelectedRole_ItemUpdated( object sender, DetailsViewUpdatedEventArgs e )
	{
		gvRoles.SelectedIndex = -1;
		gvRoles.DataBind();
	}

	#endregion
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Logger;
using UlterSystems.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;

/// <summary>
/// Control for editing groups membership.
/// </summary>
public partial class Controls_GroupsMembershipControl : System.Web.UI.UserControl
{
	#region Properties

	/// <summary>
	/// ID of user which groups membership is edited.
	/// </summary>
	public int? UserID
	{
		[DebuggerStepThrough]
		get
		{
		    if( ViewState[ "UserID" ] == null )
				return null;
		
            if( ViewState[ "UserID" ] is int )
		        return (int) ViewState[ "UserID" ];
		    
            return null;
		}
	    [DebuggerStepThrough]
		set
		{
			if( UserID != value )
			{
				ViewState[ "UserID" ] = value;
				RemovedRoles.Clear();
				ShowGroups();
			}
		}
	}

	/// <summary>
	/// List of removed roles.
	/// </summary>
    protected List<string> RemovedRoles
    {
        [DebuggerStepThrough]
        get
        {
            if (ViewState["RemovedRoles"] == null)
                ViewState["RemovedRoles"] = new List<string>();
            return ViewState["RemovedRoles"] as List<string>;
        }
    }

	#endregion

	#region Event handlers

	/// <summary>
	/// Handles control loading.
	/// </summary>
    protected override void OnLoad(EventArgs e)
	{
	    base.OnLoad(e);

	    if (!IsPostBack)
	        FillListOfGroups();
	}

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        IList<PersonAttribute> attributes =
            PersonAttributes.GetPersonAttributesByKeyword((int) UserID
                                                          , PersonAttributeTypes.PublicPassword.ToString());
        tbPassword.Text = (attributes == null || attributes.Count == 0)
                              ? String.Empty
                              : attributes[0].Value.ToString();

        ListItem listItem = lbPersonGroups.Items.FindByValue(RolesEnum.PublicUser.ToString());
        trPassword.Visible = (listItem != null);
        requiredPassword.Visible = (listItem != null);
    }

    /// <summary>
	/// Handles adding person to a group.
	/// </summary>
    protected void OnAdd(object sender, EventArgs e)
    {
        try
        {
            // Do not add the same groups.
            if (lbPersonGroups.Items.FindByValue(ddlGroups.SelectedValue) != null)
                return;

            if (RemovedRoles.Contains(ddlGroups.SelectedValue))
                RemovedRoles.Remove(ddlGroups.SelectedValue);

            lbPersonGroups.Items.Add(new ListItem(ddlGroups.SelectedItem.Text,
                                                  ddlGroups.SelectedValue));

            // Add group OfficeNewsEditor if we add group Employee
            Role officeNewsEditorRole = Role.GetRole("OfficeNewsEditor");
            Role employeeRole = Role.GetRole("Employee");

            if ((ddlGroups.SelectedValue == employeeRole.RoleID) &&
                (lbPersonGroups.Items.FindByValue(officeNewsEditorRole.RoleID) == null))
            {
                lbPersonGroups.Items.Add(new ListItem(officeNewsEditorRole.Description.ToString(),
                                                 officeNewsEditorRole.RoleID));
            }
        }
        catch (Exception ex)
        {
			Logger.Instance.Error(ex.Message, ex);
        }
    }

    /// <summary>
	/// Handles removing person from a group.
	/// </summary>
    protected void OnRemove(object sender, EventArgs e)
    {
        try
        {
            for (int i = lbPersonGroups.Items.Count - 1; i >= 0; i--)
            {
                ListItem item = lbPersonGroups.Items[i];

                if (!item.Selected)
                    continue;

                RemovedRoles.Add(item.Value);

                lbPersonGroups.Items.RemoveAt(i);
            }
        }
        catch (Exception ex)
        {
			Logger.Instance.Error(ex.Message, ex);
        }
    }

    #endregion

	#region Methods

	/// <summary>
	/// Fills list of groups.
	/// </summary>
    private void FillListOfGroups()
	{
	    try
	    {
	        ddlGroups.Items.Clear();

	        ddlGroups.DataSource = Role.GetAllRoles();
	        ddlGroups.DataBind();

	        ddlGroups.SelectedIndex = 1;
	    }
	    catch (Exception ex)
	    {
			Logger.Instance.Error(ex.Message, ex);
	    }
	}

    /// <summary>
	/// Shows groups of person.
	/// </summary>
    private void ShowGroups()
    {
        try
        {
            lbPersonGroups.Items.Clear();

            if (UserID != null)
            {
                Person p = Person.GetPersonByID(UserID.Value);
                if (p != null)
                {
                    lbPersonGroups.DataSource = p.Roles;
                    lbPersonGroups.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Instance.Error(ex.Message, ex);
        }
    }

    /// <summary>
	/// Sets membership of person to selected roles.
	/// </summary>
	/// <param name="personID">ID of person which membership is modified.</param>
    public void GenerateMembership(int personID)
    {
        try
        {
            // Check real existence of person.
            Person person = Person.GetPersonByID(personID);
            if (person == null || person.ID == null)
                return;

            // Remove person from removed roles.
            foreach (string removeRoleID in RemovedRoles)
            {
                Role role = Role.GetRole(removeRoleID);
                if (role != null
                    && role.IsInRole(personID)
                    && processDeleteUserRole(role))
                {
                    role.RemoveUser(personID);
                }
            }
            RemovedRoles.Clear();

            // Add person to selected groups.
            foreach (ListItem item in lbPersonGroups.Items)
            {
                Role role = Role.GetRole(item.Value);
                if (role == null)
                    continue;

                addUserRole(person, role);
            }
        }
        catch (Exception ex)
        {
			Logger.Instance.Error(ex.Message, ex);
        }
    }

    /// <summary>
    /// Add user to role.
    /// </summary>
    /// <param name="person">User.</param>
    /// <param name="role">Role.</param>
    private void addUserRole(Person person, Role role)
    {
        if (role.RoleID != RolesEnum.PublicUser.ToString())
        {
            if (!person.IsInRole(role.RoleID))
                role.AddUser(person.ID.Value);
            return;
        }

        if (String.IsNullOrEmpty(tbPassword.Text))
            return;

        IList<PersonAttribute> attributes =
            PersonAttributes.GetPersonAttributesByKeyword(person.ID.Value
                                                          , PersonAttributeTypes.PublicPassword.ToString());
        if (attributes == null || attributes.Count == 0)
            person.AddStandardStringAttribute(PersonAttributeTypes.PublicPassword,
                                                                     tbPassword.Text);
        else
        {
            attributes[0].StringField = tbPassword.Text;
            attributes[0].Save();
        }
        role.AddUser(person.ID.Value);
    }

    /// <summary>
    /// Make some actions before deleteing user role.
    /// </summary>
    /// <param name="role">role.</param>
    /// <returns>Access.</returns>
    private bool processDeleteUserRole(Role role)
    {
        if (role.RoleID != RolesEnum.PublicUser.ToString())
            return true;

        IList<PersonAttribute> attributes =
            PersonAttributes.GetPersonAttributesByKeyword((int) UserID
                                                          , PersonAttributeTypes.PublicPassword.ToString());
        if (attributes == null || attributes.Count == 0)
            return false;

        attributes[0].Delete();
        return true;
    }

    #endregion
}

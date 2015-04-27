using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Core;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.Persons.Filter;
using ConfirmIt.PortalLib.FiltersSupport;
using Core.ORM;
using UlterSystems.PortalLib.BusinessObjects;

public partial class UsersFilter : BaseUserControl, IFilterControl
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (IsPostBack)
            return;

        ddlRole.DataSource = Role.GetAllRoles();
        ddlRole.DataBind();

        cblEvents.DataSource = UptimeEventType.GetAllEventTypes();
        cblEvents.DataBind();

        ddlProject.DataSource = BasePlainObject.GetObjects(typeof(Project));
        ddlProject.DataBind();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        FilterChanged = false;

        btnSearch.Click += OnApplyFilter;
        btnResetFilter.Click += OnResetFilter;
    }

    #region IFilterControl support

    public bool FilterChanged
    {
        get
        {
            if (ViewState["PersonFilterChanged"] == null)
                ViewState["PersonFilterChanged"] = false;

            return (bool)ViewState["PersonFilterChanged"];
        }
        set
        {
            ViewState["PersonFilterChanged"] = value;
        }
    }

    public IFilter Filter
    {
        get { return getFilterObject(); }
    }

    private PersonsFilter getFilterObject()
    {
        List<int> selectedEventsID = new List<int>();
        foreach( ListItem item in cblEvents.Items)
        {
            if (item.Selected)
                selectedEventsID.Add(int.Parse(item.Value));
        }
        MLString firstNameMLString;
        MLString lastNameMLString;
        if (CultureManager.CurrentLanguage == CultureManager.Languages.Russian)
        {
            firstNameMLString = new MLString(tbxFirstName.Text, "");
            lastNameMLString = new MLString(tbxLastName.Text, "");
        }
        else
        {
            firstNameMLString = new MLString("", tbxFirstName.Text);
            lastNameMLString = new MLString("", tbxLastName.Text);
        }
        return new PersonsFilter
                   {
                       FirstName = firstNameMLString,
                       LastName = lastNameMLString,
                       OfficeID = int.Parse(ddlOfficeName.SelectedValue),
                       ProjectID = int.Parse(ddlProject.SelectedValue),
                       RoleID = int.Parse(ddlRole.SelectedValue),
                       Events = selectedEventsID
                   };
    }

    private void OnResetFilter(object sender, EventArgs e)
    {
        FilterChanged = true;

        tbxFirstName.Text = tbxLastName.Text = String.Empty;
        ddlOfficeName.SelectedIndex = ddlProject.SelectedIndex = ddlRole.SelectedIndex = 0;

        foreach (ListItem item in cblEvents.Items)
        {
            item.Selected = false;
        }
    }

    private void OnApplyFilter(object sender, EventArgs e)
    {
        FilterChanged = true;
    }

    #endregion
}

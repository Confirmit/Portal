using System;
using System.Collections;
using System.Web.UI.WebControls;

using UIPProcess.DataBinding;

using ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers;

public partial class DiskFilter : BaseUserControl
{
    protected override void OnInit(System.EventArgs e)
    {
        base.OnInit(e);

        btnSearch.Click += onApplyClicked;
        btnReset.Click += OnResetClicked;
    }

    protected void onApplyClicked(Object sender, EventArgs e)
    {
        Controller.OnApply(this);
    }

    public void OnResetClicked(Object sender, EventArgs e)
    {
        Controller.OnReset(this);
    }

    public new DiskFilterCtlController Controller
    {
        get { return (DiskFilterCtlController)base.Controller; }
    }

    #region [ State mapped property ]

    /// <summary>
    /// State mapped property
    /// </summary>
    public Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters.DiskFilter Filter
    {
        set { DataBinder.DataBindToControlAttributedProps(value, this); }
    }

    /// <summary>
    /// State mapped property
    /// </summary>
    public IList Offices
    {
        set
        {
            ddlOffices.Items.Clear();
            ddlOffices.Items.Add(new ListItem("Any Office", "") { Selected = true });

            ddlOffices.DataSource = value;
            ddlOffices.DataTextField = "OfficeName";
            ddlOffices.DataValueField = "Id";
            ddlOffices.DataBind();
        }
    }

    #endregion

    #region [ DataBinding ]

    [DataBinding("Title", "", false)]
    public string Title
    {
        get { return tbxTitle.Text; }
        set { tbxTitle.Text = value; }
    }

    [DataBinding("FromPublishingYear", 1900, false)]
    public int FromPublishingYear
    {
        get { return string.IsNullOrEmpty(tbxFromPublishingYear.Text) ? 1900 : int.Parse(tbxFromPublishingYear.Text); }
        set { tbxFromPublishingYear.Text = value.ToString(); }
    }

    [DataBinding("ToPublishingYear", null, false)]
    public int ToPublishingYear
    {
        get { return string.IsNullOrEmpty(tbxToPublishingYear.Text) ? DateTime.Now.Year : int.Parse(tbxToPublishingYear.Text); }
        set { tbxToPublishingYear.Text = value.ToString(); }
    }

    [DataBinding("Annotation", "", false)]
    public string Annotation
    {
        get { return tbxAnnotation.Text; }
        set { tbxAnnotation.Text = string.IsNullOrEmpty(value) ? string.Empty : value.ToString(); }
    }

    [DataBinding("Manufacturers", "", false)]
    public string Manufacturers
    {
        get { return tbxManufacturers.Text; }
        set { tbxManufacturers.Text = string.IsNullOrEmpty(value) ? string.Empty : value.ToString(); }
    }

    [DataBinding("OfficeID", 0, false)]
    public int? OfficeID
    {
        get { return string.IsNullOrEmpty(ddlOffices.SelectedValue) ? null : (int?)int.Parse(ddlOffices.SelectedValue); }
        set
        {
            if (value == null)
            {
                ddlOffices.SelectedIndex = 0;
                return;
            }

            DataBinder.SetDropDownListValue(ddlOffices, value.Value);
        }
    }

    #endregion
}
using System;
using System.Collections;

using UIPProcess.DataBinding;
using UIPProcess.DataValidating;

public partial class DiskInfo : BaseUserControl
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        menuActions.ScriptManager = ((BaseMasterPage)Page.MasterPage).ScriptManager;
        ActionsMenuClientId = menuActions.ClientID;
    }

    #region [ State Mapped Property ]

    public IList Offices
    {
        set
        {
            ddlOffices.DataTextField = "OfficeName";
            ddlOffices.DataValueField = "Id";

            DataBinder.BindDropDownDataSource(ddlOffices, value);
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

    [DataBinding("Manufacturers", "", false)]
    public string Manufacturers
    {
        get { return tbxManufacturers.Text; }
        set { tbxManufacturers.Text = value; }
    }

    [DataBinding("Annotation", "", false)]
    public string Annotation
    {
        get { return tbxAnnotation.Value; }
        set { tbxAnnotation.Value = value; }
    }

    [DataBinding("PublishingYear", 1900, false)]
    public int PublishingYear
    {
        get { return string.IsNullOrEmpty(tbxPublishingYear.Text) ? 1900 : int.Parse(tbxPublishingYear.Text); }
        set { tbxPublishingYear.Text = value.ToString(); }
    }

    [DataBinding("OfficeID", 1, false)]
    public int OfficeID
    {
        get { return string.IsNullOrEmpty(ddlOffices.SelectedValue) ? 1 : int.Parse(ddlOffices.SelectedValue); }
        set { ddlOffices.Items.FindByValue(value.ToString()).Selected = true; }
    }

    #endregion

    #region [ DataValidating ]

    [DataValidating]
    public bool ValidateDiskTitle()
    {
        reqTitle.Visible = string.IsNullOrEmpty(tbxTitle.Text);
        return !reqTitle.Visible;
    }

    [DataValidating(true)]
    public void ClearValidateTitle()
    {
        reqTitle.Visible = false;
    }

    [DataValidating]
    public bool ValidateDiskManufacturers()
    {
        reqManufacturers.Visible = string.IsNullOrEmpty(tbxManufacturers.Text);
        return !reqManufacturers.Visible;
    }

    [DataValidating(true)]
    public void ClearValidateManufacturers()
    {
        reqManufacturers.Visible = false;
    }

    [DataValidating]
    public bool ValidateDiskAnnotation()
    {
        reqAnnotation.Visible = string.IsNullOrEmpty(tbxAnnotation.Value);
        return !reqAnnotation.Visible;
    }

    [DataValidating(true)]
    public void ClearValidateAnnotation()
    {
        reqAnnotation.Visible = false;
    }

    [DataValidating]
    public bool ValidateDiskPublishingDate()
    {
        reqPublishingDate.Visible = string.IsNullOrEmpty(tbxPublishingYear.Text);
        return !reqPublishingDate.Visible;
    }

    [DataValidating(true)]
    public void ClearValidatePublishingDate()
    {
        reqPublishingDate.Visible = false;
    }

    #endregion
}
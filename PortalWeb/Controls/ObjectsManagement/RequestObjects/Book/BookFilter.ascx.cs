using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UIPProcess.DataBinding;
using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers;

public partial class BookFilterCtl : BaseUserControl
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

    public new BookFilterCtlController Controller
    {
        get { return (BookFilterCtlController)base.Controller; }
    }

    #region [ State mapped property ]

    /// <summary>
    /// State mapped property
    /// </summary>
    public BookFilter Filter
    {
        set { DataBinder.DataBindToControlAttributedProps(value, this); }
    }

    /// <summary>
    /// State mapped property
    /// </summary>
    public IList BookThemes
    {
        set
        {
            cblThemes.DataSource = value;
            cblThemes.DataTextField = "Name";
            cblThemes.DataValueField = "Id";
            cblThemes.DataBind();
        }
    }

    /// <summary>
    /// State mapped property
    /// </summary>
    public IList Languages
    {
        set
        {
            rblLanguages.Items.Clear();
            rblLanguages.Items.Add(new ListItem("Any Language", "") { Selected = true });

            rblLanguages.DataSource = value;
            rblLanguages.DataBind();
        }
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

    [DataBinding("Authors", "", false)]
    public string Authors
    {
        get { return tbxAuthors.Text; }
        set { tbxAuthors.Text = value; }
    }

    [DataBinding("Title", "", false)]
    public string Title
    {
        get { return tbxTitle.Text; }
        set { tbxTitle.Text = value; }
    }

    [DataBinding("Annotation", "", false)]
    public string Annotation
    {
        get { return tbxAnnotation.Text; }
        set { tbxAnnotation.Text = value; }
    }

    [DataBinding("FromPublishingYear", 1900, false)]
    public int FromPublishingYear
    {
        get { return int.Parse(tbxFromPublishingYear.Text); }
        set { tbxFromPublishingYear.Text = value.ToString(); }
    }

    [DataBinding("ToPublishingYear", null, false)]
    public int ToPublishingYear
    {
        get { return int.Parse(tbxToPublishingYear.Text); }
        set { tbxToPublishingYear.Text = value.ToString(); }
    }

    [DataBinding("Themes", null, false)]
    public IList<int> ThemesID
    {
        get
        {
            IList<int> selectedThemes = new List<int>();
            foreach (ListItem item in cblThemes.Items)
            {
                if (item.Selected)
                    selectedThemes.Add(int.Parse(item.Value));
            }

            return selectedThemes;
        }
        set
        {
            if (value == null)
                return;

            foreach (var val in value)
            {
                cblThemes.Items.FindByValue(val.ToString()).Selected = true;
            }
        }
    }

    [DataBinding("Language", "Any Language", false)]
    public string Language
    {
        get { return rblLanguages.SelectedValue; }
        set 
        {
            if (string.IsNullOrEmpty(value))
                return;

            rblLanguages.Items.FindByValue(value).Selected = true; 
        }
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

    [DataBinding("IsElectronic", true, false)]
    public bool IsElectronic
    {
        get { return cbIsElectronic.Checked; }
        set { cbIsElectronic.Checked = value; }
    }

    #endregion
}
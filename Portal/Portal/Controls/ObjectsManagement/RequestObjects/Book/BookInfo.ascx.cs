using System;
using System.Collections;
using System.Collections.Generic;

using UIPProcess.DataBinding;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using System.Web.UI.WebControls;
using UIPProcess.DataValidating;

public partial class BookInfo : BaseUserControl
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        menuActions.ScriptManager = ((BaseMasterPage)Page.MasterPage).ScriptManager;
        ActionsMenuClientId = menuActions.ClientID;
    }

    #region [ State Mapped Property ]

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
            ddlLanguages.DataSource = value;
            ddlLanguages.DataBind();
        }
    }

    /// <summary>
    /// State mapped property
    /// </summary>
    public IList Offices
    {
        set
        {
            ddlOffices.DataSource = value;
            ddlOffices.DataTextField = "OfficeName";
            ddlOffices.DataValueField = "Id";
            ddlOffices.DataBind();
        }
    }

    /// <summary>
    /// State mapped property
    /// </summary>
    public IList Owners
    {
        set
        {
            ddlOwners.DataSource = value;
            ddlOwners.DataTextField = "FullName";
            ddlOwners.DataValueField = "Id";
            ddlOwners.DataBind();
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

    [DataBinding("Authors", "", false)]
    public string Authors
    {
        get { return tbxAuthors.Text; }
        set { tbxAuthors.Text = value; }
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

    [DataBinding("Themes", null, false)]
    public BookTheme[] Themes
    {
        get
        {
            var selectedThemes = new List<BookTheme>();
            foreach (ListItem item in cblThemes.Items)
            {
                if (!item.Selected)
                    continue;

                BookTheme theme = new BookTheme();
                theme.Load(int.Parse(item.Value));
                selectedThemes.Add(theme);
            }

            return selectedThemes.ToArray();
        }
        set
        {
            if (value == null)
                return;

            foreach (var val in value)
            {
                cblThemes.Items.FindByValue(val.ID.ToString()).Selected = true;
            }
        }
    }

    [DataBinding("Language", null, false)]
    public string Language
    {
        get { return ddlLanguages.SelectedValue; }
        set 
        {
            if (string.IsNullOrEmpty(value))
                return;

            ddlLanguages.Items.FindByValue(value).Selected = true; 
        }
    }

    [DataBinding("OfficeID", 1, false)]
    public int OfficeID
    {
        get { return string.IsNullOrEmpty(ddlOffices.SelectedValue) ? 1 : int.Parse(ddlOffices.SelectedValue); }
        set { ddlOffices.Items.FindByValue(value.ToString()).Selected = true; }
    }

    [DataBinding("IsElectronic", true, false)]
    public bool IsElectronic
    {
        get { return cbIsElectronic.Checked; }
        set { cbIsElectronic.Checked = value; }
    }

    [DataBinding("DownloadLink", "", false)]
    public string Location
    {
        get { return tbxLocation.Text; }
        set { tbxLocation.Text = value; }
    }

    #endregion

    #region [ DataValidating ]

    [DataValidating]
    public bool ValidateBookTitle()
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
    public bool ValidateBookAuthors()
    {
        reqAuthors.Visible = string.IsNullOrEmpty(tbxAuthors.Text);
        return !reqAuthors.Visible;
    }

    [DataValidating(true)]
    public void ClearValidateAuthors()
    {
        reqAuthors.Visible = false;
    }

    [DataValidating]
    public bool ValidateBookAnnotation()
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
    public bool ValidateBookLocation()
    {
        reqLocation.Visible = string.IsNullOrEmpty(tbxLocation.Text);
        return !reqLocation.Visible;
    }

    [DataValidating(true)]
    public void ClearValidateLocation()
    {
        reqLocation.Visible = false;
    }

    [DataValidating]
    public bool ValidateBookPublishingDate()
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
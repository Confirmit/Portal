using System;
using System.Collections;
using System.Web.UI.WebControls;
using UIPProcess.DataBinding;
using UIPProcess.DataValidating;

using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;

public partial class CardInfo : BaseUserControl
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        menuActions.ScriptManager = ((BaseMasterPage)Page.MasterPage).ScriptManager;
        ActionsMenuClientId = menuActions.ClientID;
    }

    #region [ State mapped property ]

    /// <summary>
    /// State mapped property
    /// </summary>
    public IList Offices
    {
        set
        {
            ddlOffices.DataTextField = "OfficeName";
            ddlOffices.DataValueField = "Id";

            DataBinder.BindDropDownDataSource(ddlOffices, value);
        }
    }

    /// <summary>
    /// State mapped property
    /// </summary>
    public IList Owners
    {
        set
        {
            ddlOwner.DataTextField = "FullName";
            ddlOwner.DataValueField = "Id";

            DataBinder.BindDropDownDataSource(ddlOwner, value);
            ddlOwner.Items.Insert(0, new ListItem(string.Empty, string.Empty));
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

    [DataBinding("ShopName", "", false)]
    public string ShopName
    {
        get { return tbxShopName.Text; }
        set { tbxShopName.Text = value; }
    }

    [DataBinding("ShopSite", "", false)]
    public string ShopSite
    {
        get { return tbxShopSite.Text; }
        set { tbxShopSite.Text = value; }
    }

    [DataBinding("ValuePercent", 0, false)]
    public int ValuePercent
    {
        get { return string.IsNullOrEmpty(tbxValuePercent.Text) ? 0 : int.Parse(tbxValuePercent.Text); }
        set { tbxValuePercent.Text = value.ToString(); }
    }

    [DataBinding("OfficeID", 1, false)]
    public int OfficeID
    {
        get { return string.IsNullOrEmpty(ddlOffices.SelectedValue) ? 1 : int.Parse(ddlOffices.SelectedValue); }
        set { ddlOffices.Items.FindByValue(value.ToString()).Selected = true; }
    }

    [DataBinding("OwnerID", null, false)]
    public int? OwnerID
    {
        get { return string.IsNullOrEmpty(ddlOwner.SelectedValue) ? null : (int?)int.Parse(ddlOwner.SelectedValue); }
        set { ddlOwner.Items.FindByValue(value == null ? string.Empty : value.ToString()).Selected = true; }
    }

    #endregion

    #region [ DataValidating ]

    [DataValidating]
    public bool ValidateTitle()
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
    public bool ValidateShopName()
    {
        reqShopName.Visible = string.IsNullOrEmpty(tbxShopName.Text);
        return !reqShopName.Visible;
    }

    [DataValidating(true)]
    public void ClearValidateShopName()
    {
        reqShopName.Visible = false;
    }

    #endregion
}
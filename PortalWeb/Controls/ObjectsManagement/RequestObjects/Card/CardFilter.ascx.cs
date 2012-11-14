using System;
using System.Collections;
using System.Web.UI.WebControls;

using UIPProcess.DataBinding;

using ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers;

public partial class CardFilter : BaseUserControl
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

    public new CardFilterCtlController Controller
    {
        get { return (CardFilterCtlController)base.Controller; }
    }

    #region [ State mapped property ]

    /// <summary>
    /// State mapped property
    /// </summary>
    public Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters.CardFilter Filter
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

    [DataBinding("ShopName", "", false)]
    public string ShopName
    {
        get { return tbxShopName.Text; }
        set { tbxShopName.Text = value; }
    }

    [DataBinding("ValuePercent", null, false)]
    public int? ValuePercent
    {
        get { return string.IsNullOrEmpty(tbxValuePercent.Text) ? (int?)null : int.Parse(tbxValuePercent.Text); }
        set { tbxValuePercent.Text = value == null ? string.Empty : value.ToString(); }
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
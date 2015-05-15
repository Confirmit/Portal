using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.Arrangements;
using ConfirmIt.PortalLib.Logger;
using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.BAL;

using Office = UlterSystems.PortalLib.BusinessObjects.Office;

public partial class Arrangements_AddEditConferenceHall : BaseWebPage
{
    #region Fields
    private XMLSerializableConferenceHall m_currentCH;
    #endregion

    #region Properties
    /// <summary>
    /// Current conference hall
    /// </summary>
    virtual public XMLSerializableConferenceHall CurrentCH
    {
        get
        {
            if (m_currentCH == null)
            {
                m_currentCH = new XMLSerializableConferenceHall();
                if (ConferenceHallID.HasValue)
                {
                    try
                    {
                        // Create service wrapper.
                        Office of = new Office();
                        of.Load(OfficeID.Value);
                        using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
                        {
                            m_currentCH = c.GetConferenceHall(ConferenceHallID.Value);
                        }
                    }
                    catch (Exception ex)
                    {
						Logger.Instance.Error(ex.Message, ex);
                    }
                }

            }
            return m_currentCH;
        }
        set
        {
            m_currentCH = value;
        }
    }

    /// <summary>
    /// ID of current conference hall.
    /// </summary>
    virtual public int? ConferenceHallID
    {
        get
        {
            int value;
            if (Int32.TryParse(Request.Params["id"], out value))
            {
                return value;
            }
            else
            {

                return null;
            }
        }
    }
    /// <summary>
    /// ID of current office.
    /// </summary>
    virtual public int? OfficeID
    {
        get
        {
            int value;
            if (Int32.TryParse(Request.Params["officeid"], out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }
    /// <summary>
    /// Is conerence hall in preview mode
    /// </summary>
    public bool IsPreview
    {
        get
        {
            if (ConferenceHallID.HasValue)
            {
                if (!(CurrentUser.IsInRole(RolesEnum.OfficeArrangementsEditor) || CurrentUser.IsInRole(RolesEnum.GeneralArrangementsEditor)))
                    return true;
                //if ((CurrentUser.IsInRole(RolesEnum.OfficeArrangementsEditor)) && (OfficeID.Value != 0))
                //    return true;
            }
            return false;
        }
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser == null)
            Response.Redirect("~//Admin//AdminArrangements//AdminArrangementsPage.aspx");

        if (!Page.IsPostBack)
        {
            if (ConferenceHallID.HasValue)                            //edit
            {
                if (IsPreview)
                {
                    lTitle.Text = GetLocalResourceObject("tPreview").ToString() + ":";
                    SetControlsProp(false);
                    btnDelete.Visible = false;
                }
                else
                {
                    lTitle.Text = GetLocalResourceObject("tEdit").ToString() + ":";
                    SetControlsProp(true);
                    btnDelete.Visible = true;
                }
                LoadCH();
            }
            else                                            //add
            {
                lTitle.Text = GetLocalResourceObject("tNew").ToString() + ":";
                SetControlsProp(true);
                btnDelete.Visible = false;
                Office of = new Office();
                of.Load(OfficeID.Value);

                //list of offices
                Office[] collection = Office.GetOffices();
                List<XMLSerializableOffice> coll = new List<XMLSerializableOffice>();
                foreach (Office office in collection)
                {
                    coll.Add(new XMLSerializableOffice(office));
                }
                tbOffice.DataSource = coll.ToArray();
                tbOffice.DataBind();
                if (of.OfficeName != "")
                {
                    tbOffice.SelectedIndex = tbOffice.Items.IndexOf(tbOffice.Items.FindByValue(of.OfficeName));
                }
            }
        }
    }
    /// <summary>
    /// Apply button click.
    /// </summary>
    protected void btnApply_Click(object sender, EventArgs e)
    {
        FillCH();
        try
        {
            // Create service wrapper.
            Office of = new Office();
            of.Load(OfficeID.Value);
            using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
            {
                if (ConferenceHallID.HasValue)
                    c.EditConferenceHall(CurrentCH.ConferenceHallID, CurrentCH.Name, CurrentCH.Description, CurrentCH.OfficeID);
                else
                    c.AddConferenceHall(CurrentCH.Name, CurrentCH.Description, CurrentCH.OfficeID);
            }

        }
        catch (Exception ex)
        {
			Logger.Instance.Error(ex.Message, ex);
        }
        Response.Redirect("~//Admin//AdminArrangements//AdminArrangementsPage.aspx");
    }
    /// <summary>
    /// Delete button click.
    /// </summary>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            // Create service wrapper.
            Office of = new Office();
            of.Load(OfficeID.Value);
            using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
            {
                c.DeleteConferenceHall(CurrentCH.ConferenceHallID);
            }
        }
        catch (Exception ex)
        {
			Logger.Instance.Error(ex.Message, ex);
        }

        Response.Redirect("~//Admin//AdminArrangements//AdminArrangementsPage.aspx");
    }
    #endregion

    #region Methods
    /// <summary>
    /// Fill conference hall object.
    /// </summary>
    protected void FillCH()
    {
        CurrentCH.OfficeID = tbOffice.SelectedIndex + 1;
        CurrentCH.Name = tbCHName.Text;
        CurrentCH.Description = tbDescription.Text;
    }
    /// <summary>
    /// Load conference hall object.
    /// </summary>
    protected void LoadCH()
    {
        Office of = new Office();
        of.Load(OfficeID.Value);
        Office[] collection = Office.GetOffices();
        List<XMLSerializableOffice> coll = new List<XMLSerializableOffice>();
        foreach (Office office in collection)
        {
            coll.Add(new XMLSerializableOffice(office));
        }
        tbOffice.DataSource = coll.ToArray();
        tbOffice.DataBind();
        if (of.OfficeName != "")
        {
            tbOffice.SelectedIndex = tbOffice.Items.IndexOf(tbOffice.Items.FindByValue(of.OfficeName));
        }
        tbCHName.Text = CurrentCH.Name;
        tbDescription.Text = CurrentCH.Description;
    }
    /// <summary>
    /// Set or remove ability of editing.
    /// </summary>
    protected void SetControlsProp(bool CanEdit)
    {
        tbOffice.Enabled = CanEdit;
        tbCHName.ReadOnly = !CanEdit;
        tbDescription.ReadOnly = !CanEdit;
        btnApply.Visible = CanEdit;
    }
    #endregion
}

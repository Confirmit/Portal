using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using Core;
using ConfirmIt.Portal.WcfServiceLibrary;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.Arrangements;

using Office = UlterSystems.PortalLib.BusinessObjects.Office;


public partial class Arrangements_AddEditArrangement : BaseWebPage
{
    #region Fields
    private XMLSerializableArrangement m_currentArrangement; 
    #endregion

    #region Properties
    /// <summary>
    /// Current arrangement
    /// </summary>
    virtual public XMLSerializableArrangement CurrentArrangement
    {
        get
        {
            if (m_currentArrangement == null)
            {
                m_currentArrangement = new XMLSerializableArrangement();
                if (ArrangementID.HasValue)
                {
                    try
                    {
                        // Create service wrapper.
                        Office of = new Office();
                        of.Load(OfficeID.Value);
                        using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
                        {
                            m_currentArrangement = c.GetArrangement(ArrangementID.Value);
                        }
                    }
                    catch (Exception ex)
                    { 
                        UlterSystems.PortalLib.Logger.Log.Error(ex.Message, ex);
                    }
                }

            }
            return m_currentArrangement;
        }
        set
        {
            m_currentArrangement = value;
        }
    }

    /// <summary>
    /// ID of current arrangement.
    /// </summary>
    virtual public int? ArrangementID
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
    /// Is arrangement in preview mode
    /// </summary>
    public bool IsPreview
    {
        get {return true;}
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser == null)
            Response.Redirect("~//Arrangements//Default.aspx");
        
        if (!Page.IsPostBack)                               
        {
            if (ArrangementID.HasValue)                            //edit
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
                LoadArrangement();
            }
            else                                            //add
            {
                lTitle.Text = GetLocalResourceObject("tNew").ToString() + ":";
                SetControlsProp(true);
                btnDelete.Visible = false;
                Office of = new Office();
                of.Load(OfficeID.Value);
                this.tbOffice.Text = of.OfficeName;
                FillConfHallList();
                this.Calendar.SelectedDate = DateTime.Today;
                this.tbTimeBegin.Text = DateTime.Now.ToShortTimeString();
                this.tbTimeEnd.Text = DateTime.Now.ToShortTimeString();
            }
        }
    }
    /// <summary>
    /// Apply button click.
    /// </summary>
    protected void btnApply_Click(object sender, EventArgs e)
    {
        cvTimeBegin.Validate();
        cvTimeEnd.Validate();
        cvCheckAdding.Validate();
        if (!Page.IsValid)
            return;
        FillArrangement();
        try
        {
            // Create service wrapper.
            Office of = new Office();
            of.Load(OfficeID.Value);
            using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
            {
                if (ArrangementID.HasValue)
                    c.EditArrangement(CurrentArrangement.ArrangementID, CurrentArrangement.Name, CurrentArrangement.Description, CurrentArrangement.ConferenceHallID, CurrentArrangement.TimeBegin, CurrentArrangement.TimeEnd, CurrentArrangement.ListOfGuests, CurrentArrangement.Equipment);
                else
                    c.AddArrangement(CurrentArrangement.Name, CurrentArrangement.Description, CurrentArrangement.ConferenceHallID, CurrentArrangement.TimeBegin, CurrentArrangement.TimeEnd, CurrentArrangement.ListOfGuests, CurrentArrangement.Equipment);
            }
        }
        catch (Exception ex)
        { UlterSystems.PortalLib.Logger.Log.Error(ex.Message, ex); }
        Response.Redirect("~//Arrangements//Default.aspx");
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
                c.DeleteArrangement(CurrentArrangement.ArrangementID);
            }
        }
        catch (Exception ex)
        { UlterSystems.PortalLib.Logger.Log.Error(ex.Message, ex); }
        Response.Redirect("~//Arrangements//Default.aspx");
    }
    /// <summary>
    /// Validate TimeBegin string;
    /// </summary>
    protected void cvTimeBegin_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = CheckTime(args.Value);
    }
    /// <summary>
    /// Validate TimeEnd string.
    /// </summary>
    protected void cvTimeEnd_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if(CheckTime(args.Value))
        {
            if(TimeSpan.Compare(TimeSpan.Parse(tbTimeEnd.Text),TimeSpan.Parse(tbTimeBegin.Text))<1)
            {
                args.IsValid = false;
                return;
            }
        }
        else
        {
            args.IsValid = false;
            return;
        }
        args.IsValid = true;
    }
    /// <summary>
    /// Validate adding.
    /// </summary>
    protected void  cvCheckAdding_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int ConfHallID = Int32.Parse(ddlConferenceHalls.SelectedItem.Value);
        DateTime dBegin = Calendar.SelectedDate.Add(TimeSpan.Parse(tbTimeBegin.Text));
        DateTime dEnd = Calendar.SelectedDate.Add(TimeSpan.Parse(tbTimeEnd.Text));
        int ArrID = 0;
        if (ArrangementID.HasValue)
            ArrID = ArrangementID.Value;
        try
        {
            // Create service wrapper.
            Office of = new Office();
            of.Load(OfficeID.Value);
            using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
            {
                args.IsValid = c.CheckArrangementAdding(ConfHallID, ArrID, dBegin, dEnd);
            }
        }
        catch (Exception ex)
        { UlterSystems.PortalLib.Logger.Log.Error(ex.Message, ex); }
    }
    /// <summary>
    /// Check office.
    /// </summary>
    protected void cvCheckOffice_ServerValidate(object source, ServerValidateEventArgs args)
    {
        //args.IsValid = (Int32.Parse(ddlOffices.SelectedItem.Value) == Int32.Parse(ConfigurationManager.AppSettings["OfficeID"]));
    }
    #endregion

    #region Methods
    /// <summary>
    /// Fill arrangement object.
    /// </summary>
    protected void FillArrangement()
    {
        CurrentArrangement.ConferenceHallID = Int32.Parse(ddlConferenceHalls.SelectedItem.Value);
        CurrentArrangement.Name = tbArrName.Text;
        CurrentArrangement.Description = tbDescription.Text;
        CurrentArrangement.TimeBegin = Calendar.SelectedDate.Add(TimeSpan.Parse(tbTimeBegin.Text));
        DateTime time = DateTime.Parse(tbTimeEnd.Text);
        TimeSpan ts = new TimeSpan(time.Hour,time.Minute,0);
        if ((time.Minute > 0) && (time.Minute < 15))
        {
            ts = new TimeSpan(time.Hour,15,0);
        }
        if ((time.Minute > 15) && (time.Minute < 30))
        {
            ts = new TimeSpan(time.Hour,30,0);
        }
        if ((time.Minute > 30) && (time.Minute < 45))
        {
            ts = new TimeSpan(time.Hour,45,0);
        }
        if ((time.Minute > 45) && (time.Minute < 60))
        {
            ts = new TimeSpan(time.Hour + 1, 0, 0);
        }
        CurrentArrangement.TimeEnd = Calendar.SelectedDate.Add(ts);
        CurrentArrangement.ListOfGuests = tbListOfGuests.Text;
        CurrentArrangement.Equipment = tbEquipment.Text;
    }
    /// <summary>
    /// Load arrangement object.
    /// </summary>
    protected void LoadArrangement()
    {
        tbOffice.Text = CurrentArrangement.OfficeName;
        FillConfHallList();
        tbArrName.Text = CurrentArrangement.Name;
        tbDescription.Text = CurrentArrangement.Description;
        Calendar.SelectedDate = CurrentArrangement.TimeBegin.Date;
        tbTimeBegin.Text = CurrentArrangement.TimeBegin.ToShortTimeString();
        tbTimeEnd.Text = CurrentArrangement.TimeEnd.ToShortTimeString();
        tbListOfGuests.Text = CurrentArrangement.ListOfGuests;
        tbEquipment.Text = CurrentArrangement.Equipment;
    }
    /// <summary>
    /// Function for validating of time string.
    /// </summary>
    protected bool CheckTime(string time)
    {
        try
        {
            TimeSpan.Parse(time);
            if ((DateTime.Parse(time).Hour < 9) || (DateTime.Parse(time).Hour > 20))
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    /// <summary>
    /// Filling drop down list of conference halls.
    /// </summary>
    protected void FillConfHallList()
    {
        try
        {
            // Create service wrapper.
            Office of = new Office();
            of.Load(OfficeID.Value);
            using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
            {
                XMLSerializableConferenceHall[] chs = c.GetConferenceHallsList(OfficeID.Value);
                List<XMLSerializableConferenceHall> coll = new List<XMLSerializableConferenceHall>();
                if (chs != null)
                {
                    coll.AddRange(chs);
                }
                ddlConferenceHalls.DataSource = coll.ToArray();
                ddlConferenceHalls.DataBind();
                if (ArrangementID.HasValue)
                {
                    ddlConferenceHalls.SelectedIndex = ddlConferenceHalls.Items.IndexOf(ddlConferenceHalls.Items.FindByValue(CurrentArrangement.ConferenceHallID.ToString()));
                }
            }
        }
        catch (Exception ex)
        { UlterSystems.PortalLib.Logger.Log.Error(ex.Message, ex); }
    }
    /// <summary>
    /// Set or remove ability of editing.
    /// </summary>
    protected void SetControlsProp(bool CanEdit)
    {
        tbOffice.ReadOnly = true;
        ddlConferenceHalls.Enabled = CanEdit;
        tbArrName.ReadOnly = !CanEdit;
        tbDescription.ReadOnly = !CanEdit;
        tbEquipment.ReadOnly = !CanEdit;
        tbListOfGuests.ReadOnly = !CanEdit;
        tbTimeBegin.ReadOnly = !CanEdit;
        tbTimeEnd.ReadOnly = !CanEdit;
        Calendar.Enabled = CanEdit;
        btnApply.Visible = CanEdit;
    }
    #endregion
}

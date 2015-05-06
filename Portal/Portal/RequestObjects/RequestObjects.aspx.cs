using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib;
using System.ComponentModel;
using UlterSystems.PortalLib.BusinessObjects;
using Office = ConfirmIt.PortalLib.BAL.Office;

public partial class RequestObjects : BaseWebPage
{
    #region Fields
    /// <summary>
    /// Can user edit book data.
    /// </summary>
    private bool m_CanEdit = false;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        m_CanEdit = false;
        if (CurrentUser != null)
            m_CanEdit = CurrentUser.IsInRole("Administrator");

        //if (!IsPostBack)
        //    applyBookSettings();
    }

//    protected void ddlObjectsType_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        //switch (ddlObjectTypes.SelectedIndex)
//        //{
//        //    case 0:
//        //        applyBookSettings();
//        //        break;
//        //    case 1:
//        //        applyDiskSettings();
//        //        break;
//        //    case 2:
//        //        applyCardSettings();
//        //        break;
//        //}
//    }

//    protected void dsObjects_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
//    {
//        //ObjectFilter filter = new ObjectFilter();
//        //switch (ddlObjectTypes.SelectedIndex)
//        //{
//        //    case 0:
//        //        BookFilter book_filter = new BookFilter();
//        //        book_filter.Authors = tbxAuthors.Text;
//        //        book_filter.Annotation = tbxAnnotation.Text;

//        //        List<int> selectedThemes = new List<int>();
//        //        foreach (ListItem item in cblThemes.Items)
//        //        {
//        //            if (item.Selected)
//        //                selectedThemes.Add(Convert.ToInt32(item.Value));
//        //        }
//        //        book_filter.Themes = selectedThemes.ToArray();

//        //        book_filter.FromYear = Convert.ToInt32(tbxFromPublishingYear.Text);
//        //        book_filter.ToYear = string.IsNullOrEmpty(tbxToPublishingYear.Text) ? DateTime.Now.Year : Convert.ToInt32(tbxToPublishingYear.Text);
//        //        book_filter.Language = rblLanguages.SelectedIndex == 0 ? null : rblLanguages.SelectedValue;
//        //        book_filter.IsElectronic = cbIsElectronic.Checked;
//        //        book_filter.IsPaper = cbIsPaper.Checked;
//        //        filter = (ObjectFilter)book_filter;
//        //        break;
//        //    case 1:
//        //        DiskFilter disk_filter = new DiskFilter();
//        //        disk_filter.Manufacturers = tbxManufacturers.Text;
//        //        disk_filter.Annotation = tbxAnnotation.Text;
//        //        disk_filter.FromYear = Convert.ToInt32(tbxFromPublishingYear.Text);
//        //        disk_filter.ToYear = string.IsNullOrEmpty(tbxToPublishingYear.Text) ? DateTime.Now.Year : Convert.ToInt32(tbxToPublishingYear.Text);
//        //        filter = (ObjectFilter)disk_filter;
//        //        break;
//        //    case 2:
//        //        CardFilter card_filter = new CardFilter();
//        //        card_filter.ValuePercent = tbxValuePercent.Text != string.Empty ? Convert.ToByte(tbxValuePercent.Text) : (byte)0;
//        //        card_filter.ShopName = tbxShopName.Text;
//        //        card_filter.ShopSite = tbxShopSite.Text;
//        //        filter = (ObjectFilter)card_filter;
//        //        break;
//        //}

//        //filter.Title = tbxTitle.Text;
//        //filter.OfficeID = ddlOffices.SelectedIndex == 0 ? -1 : Convert.ToInt32(ddlOffices.SelectedValue);
//        //if (ddlOwners.SelectedIndex == 0)
//        //    filter.OwnerID = -1;
//        //else if (ddlOwners.SelectedIndex == 1)
//        //    filter.OwnerID = null;
//        //else
//        //    filter.OwnerID = Convert.ToInt32(ddlOwners.SelectedValue);

//        //e.InputParameters["filter"] = filter;
//    }

//    #region Events of grid view
//    protected void gvObjects_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        if (m_CanEdit)
//        {
//            dvSelectedBook.ChangeMode(DetailsViewMode.Edit);
//            dvSelectedDisk.ChangeMode(DetailsViewMode.Edit);
//            dvSelectedCard.ChangeMode(DetailsViewMode.Edit);
//        }
//        else
//        {
//            dvSelectedBook.ChangeMode(DetailsViewMode.ReadOnly);
//            dvSelectedDisk.ChangeMode(DetailsViewMode.ReadOnly);
//            dvSelectedCard.ChangeMode(DetailsViewMode.ReadOnly);
//        }
//    }
//    protected void gvObjects_RowCreated(object sender, GridViewRowEventArgs e)
//    {
//        //if (e.Row.RowType == DataControlRowType.DataRow)
//        //{
//        //    Book book = e.Row.DataItem as Book;

//        //    ImageButton btn = e.Row.Cells[e.Row.Cells.Count - 1].Controls[0] as ImageButton;
//        //    if (btn != null)
//        //    {
//        //        btn.ID = "btnGVDelete";
//        //        btn.OnClientClick = string.Format("if (confirm('{0}') == false) return false; ", this.GetLocalResourceObject("ConfirmDeleteMessage"));
//        //    }

//        //    btn = e.Row.Cells[e.Row.Cells.Count - 2].Controls[0] as ImageButton;
//        //    if ((btn != null) && (book != null))
//        //    {
//        //        btn.ID = "btnGVSelect";
//        //    }

//        //    btn = e.Row.Cells[e.Row.Cells.Count - 3].Controls[0] as ImageButton;
//        //    if ((btn != null) && (book != null))
//        //    {
//        //        btn.ID = "btnGVDownload";

//        //        Office office = Office.GetOfficeByID(book.OfficeID);
//        //        string message =
//        //            string.Format(
//        //                ((string)this.GetLocalResourceObject("Office.Text")) + @": {0}\n" + ((string)this.GetLocalResourceObject("Download.Text")) + ": {1}",
//        //                (office == null) ? string.Empty : office.OfficeName,
//        //                Server.HtmlEncode(book.DownloadLink.Replace(@"\", @"\\")));

//        //        btn.OnClientClick = string.Format("alert('{0}'); return false; ", message);
//        //    }
//        //}
//    }
//    protected void gvObjects_RowDeleted(object sender, GridViewDeletedEventArgs e)
//    {
//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();

//        if (m_CanEdit)
//        {
//            dvSelectedBook.ChangeMode(DetailsViewMode.Insert);
//            dvSelectedDisk.ChangeMode(DetailsViewMode.Insert);
//            dvSelectedCard.ChangeMode(DetailsViewMode.Insert);
//        }
//        else
//        {
//            dvSelectedBook.ChangeMode(DetailsViewMode.ReadOnly);
//            dvSelectedDisk.ChangeMode(DetailsViewMode.ReadOnly);
//            dvSelectedCard.ChangeMode(DetailsViewMode.ReadOnly);
//        }
//    }
//    protected void gvObjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
//    {
//        if (gvObjects.SelectedIndex != -1)
//        {
//            gvObjects.SelectedIndex = -1;
//            //gvObjects.DataBind();

//            if (m_CanEdit)
//            {
//                dvSelectedBook.ChangeMode(DetailsViewMode.Insert);
//                dvSelectedDisk.ChangeMode(DetailsViewMode.Insert);
//                dvSelectedCard.ChangeMode(DetailsViewMode.Insert);
//            }
//            else
//            {
//                dvSelectedBook.ChangeMode(DetailsViewMode.ReadOnly);
//                dvSelectedDisk.ChangeMode(DetailsViewMode.ReadOnly);
//                dvSelectedCard.ChangeMode(DetailsViewMode.ReadOnly);
//            }
//        }
//    }
//    protected void gvObjects_DataBound(object sender, EventArgs e)
//    {
//        GridViewRow topPagerRow = gvObjects.TopPagerRow;
//        GridViewRow bottomPagerRow = gvObjects.BottomPagerRow;

//        ShowPagerData(topPagerRow);
//        ShowPagerData(bottomPagerRow);
//    }
//    #endregion

//    #region Events of books details view
//    protected void dvSelectedBook_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
//    {
//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();
//    }
//    protected void dvSelectedBook_ItemCommand(object sender, DetailsViewCommandEventArgs e)
//    {
//        if (e.CommandName == "Cancel")
//        {
//            gvObjects.SelectedIndex = -1;
//            //gvObjects.DataBind();
//        }
//    }
//    protected void dvSelectedBook_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
//    {
//        //int bookId = (int)dvSelectedBook.DataKey.Value;

//        //CheckBoxList cbl = dvSelectedBook.FindControl("cblDVBookThemes") as CheckBoxList;
//        //if (cbl != null)
//        //{
//        //    List<int> themeIDs = new List<int>();
//        //    foreach (ListItem item in cbl.Items)
//        //    {
//        //        if (item.Selected)
//        //            themeIDs.Add(Convert.ToInt32(item.Value));
//        //    }

//        //    Book.SetThemes(bookId, themeIDs.ToArray());
//        //}

//        //gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();
//    }
//    protected void dvSelectedBook_DataBound(object sender, EventArgs e)
//    {
//        //Book book = dvSelectedBook.DataItem as Book;
//        //if (book == null)
//        //{
//        //    return;
//        //}

//        //DropDownList ddl =
//        //    dvSelectedBook.FindControl("ddlDVLanguage") as DropDownList;
//        //if (ddl != null)
//        //{
//        //    ddl.SelectedValue = book.Language;
//        //}

//        //ddl = dvSelectedBook.FindControl("ddlDVOffices") as DropDownList;
//        //if (ddl != null)
//        //{
//        //    ddl.SelectedValue = book.OfficeID.ToString();
//        //}

//        ////@idm{
//        //ddl = dvSelectedBook.FindControl("ddlDVOwners") as DropDownList;
//        //if (ddl != null)
//        //{
//        //    if (book.OwnerID == null)
//        //        ddl.SelectedValue = "0";
//        //    else
//        //        ddl.SelectedValue = book.OwnerID.ToString();
//        //}
//        ////@idm}

//        //CheckBoxList cbl = dvSelectedBook.FindControl("cblDVBookThemes") as CheckBoxList;
//        //if (cbl != null)
//        //{
//        //    BookTheme[] themes = book.Themes;
//        //    List<int> themeIDs = new List<int>(themes.Length);
//        //    foreach (BookTheme theme in themes)
//        //    {
//        //        themeIDs.Add(theme.ID.Value);
//        //    }
//        //    foreach (ListItem item in cbl.Items)
//        //    {
//        //        item.Selected = themeIDs.Contains(Convert.ToInt32(item.Value));
//        //    }
//        //}
//    }
//    protected void dvSelectedBook_ItemInserting(object sender, DetailsViewInsertEventArgs e)
//    {
//        if (!m_CanEdit)
//        {
//            e.Cancel = true;
//            return;
//        }

//        DropDownList ddlLanguage =
//            dvSelectedBook.FindControl("ddlDVLanguage") as DropDownList;
//        if (ddlLanguage != null)
//        {
//            e.Values["Language"] = ddlLanguage.SelectedValue;
//        }

//        CheckBoxList cbl = dvSelectedBook.FindControl("cblDVBookThemes") as CheckBoxList;
//        if (cbl != null)
//        {
//            List<int> themeIDs = new List<int>();
//            foreach (ListItem item in cbl.Items)
//            {
//                if (item.Selected)
//                {
//                    themeIDs.Add(Convert.ToInt32(item.Value));
//                }
//            }
//            e.Values["Themes"] = themeIDs.ToArray();
//        }

//        DropDownList ddl = dvSelectedBook.FindControl("ddlDVOffices") as DropDownList;
//        if (ddl != null)
//        {
//            e.Values["OfficeID"] = Convert.ToInt32(ddl.SelectedValue);
//        }

//        ddl = dvSelectedBook.FindControl("ddlDVOwners") as DropDownList;
//        if (ddl != null)
//        {
//            int owner_id = Convert.ToInt32(ddl.SelectedValue);
//            if (owner_id == 0)
//                e.Values["OwnerID"] = null;
//            else
//                e.Values["OwnerID"] = owner_id;
//        }
//    }
//    protected void dvSelectedBook_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
//    {
//        if (!m_CanEdit)
//        {
//            e.Cancel = true;
//            return;
//        }

//        DropDownList ddlLanguage =
//            dvSelectedBook.FindControl("ddlDVLanguage") as DropDownList;
//        if (ddlLanguage != null)
//        {
//            e.NewValues["Language"] = ddlLanguage.SelectedValue;
//        }

//        DropDownList ddl = dvSelectedBook.FindControl("ddlDVOffices") as DropDownList;
//        if (ddl != null)
//        {
//            e.NewValues["OfficeID"] = Convert.ToInt32(ddl.SelectedValue);
//        }

//        ddl = dvSelectedBook.FindControl("ddlDVOwners") as DropDownList;
//        if (ddl != null)
//        {
//            int owner_id = Convert.ToInt32(ddl.SelectedValue);
//            if (owner_id == 0)
//                e.NewValues["OwnerID"] = null;
//            else
//                e.NewValues["OwnerID"] = owner_id;
//        }
//    }
//    protected void dvSelectedBook_ItemCreated(object sender, EventArgs e)
//    {
//        if (dvSelectedBook.CurrentMode == DetailsViewMode.Insert)
//        {
//            CheckBox cbIsElectronicEdit =
//                dvSelectedBook.FindControl("cbDVIsElectronicEdit") as CheckBox;
//            if (cbIsElectronicEdit != null)
//            {
//                cbIsElectronicEdit.Checked = true;
//            }

//            TextBox tbxDownloadLink =
//                dvSelectedBook.FindControl("tbxDVDownloadLink") as TextBox;
//            if (tbxDownloadLink != null)
//            {
//                tbxDownloadLink.Text = Globals.Settings.RequestObjects.DownloadBasePath;
//            }

//            TextBox tbxPublishingYear =
//                dvSelectedBook.FindControl("tbxDVPublishingYear") as TextBox;
//            if (tbxPublishingYear != null)
//            {
//                tbxPublishingYear.Text = DateTime.Now.Year.ToString();
//            }
//        }
//    }
//    #endregion

//    #region Events of disks details view
//    protected void dvSelectedDisk_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
//    {
//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();
//    }
//    protected void dvSelectedDisk_ItemCommand(object sender, DetailsViewCommandEventArgs e)
//    {
//        if (e.CommandName == "Cancel")
//        {
//            gvObjects.SelectedIndex = -1;
//            //gvObjects.DataBind();
//        }
//    }
//    protected void dvSelectedDisk_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
//    {
//        int DiskId = (int)dvSelectedDisk.DataKey.Value;

//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();
//    }
//    protected void dvSelectedDisk_DataBound(object sender, EventArgs e)
//    {
//        //Disk disk = dvSelectedDisk.DataItem as Disk;
//        //if (disk == null)
//        //{
//        //    return;
//        //}

//        //DropDownList ddl =
//        //    dvSelectedDisk.FindControl("ddlDVOffices") as DropDownList;
//        //if (ddl != null)
//        //{
//        //    ddl.SelectedValue = disk.OfficeID.ToString();
//        //}

//        //ddl = dvSelectedDisk.FindControl("ddlDVOwners") as DropDownList;
//        //if (ddl != null)
//        //{
//        //    if (disk.OwnerID == null)
//        //        ddl.SelectedValue = "0";
//        //    else
//        //        ddl.SelectedValue = disk.OwnerID.ToString();
//        //}
//    }
//    protected void dvSelectedDisk_ItemInserting(object sender, DetailsViewInsertEventArgs e)
//    {
//        if (!m_CanEdit)
//        {
//            e.Cancel = true;
//            return;
//        }

//        DropDownList ddl = dvSelectedDisk.FindControl("ddlDVOffices") as DropDownList;
//        if (ddl != null)
//        {
//            e.Values["OfficeID"] = Convert.ToInt32(ddl.SelectedValue);
//        }

//        ddl = dvSelectedDisk.FindControl("ddlDVOwners") as DropDownList;
//        if (ddl != null)
//        {
//            int owner_id = Convert.ToInt32(ddl.SelectedValue);
//            if (owner_id == 0)
//                e.Values["OwnerID"] = null;
//            else
//                e.Values["OwnerID"] = owner_id;
//        }
//    }
//    protected void dvSelectedDisk_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
//    {
//        if (!m_CanEdit)
//        {
//            e.Cancel = true;
//            return;
//        }

//        DropDownList ddl = dvSelectedDisk.FindControl("ddlDVOffices") as DropDownList;
//        if (ddl != null)
//        {
//            e.NewValues["OfficeID"] = Convert.ToInt32(ddl.SelectedValue);
//        }

//        ddl = dvSelectedDisk.FindControl("ddlDVOwners") as DropDownList;
//        if (ddl != null)
//        {
//            int owner_id = Convert.ToInt32(ddl.SelectedValue);
//            if (owner_id == 0)
//                e.NewValues["OwnerID"] = null;
//            else
//                e.NewValues["OwnerID"] = owner_id;
//        }
//    }
//    protected void dvSelectedDisk_ItemCreated(object sender, EventArgs e)
//    {
//        if (dvSelectedDisk.CurrentMode == DetailsViewMode.Insert)
//        {
//            TextBox tbxPublishingYear =
//                dvSelectedDisk.FindControl("tbxDVPublishingYear") as TextBox;
//            if (tbxPublishingYear != null)
//            {
//                tbxPublishingYear.Text = DateTime.Now.Year.ToString();
//            }
//        }
//    }
//    #endregion

//    #region Events of cards details view
//    protected void dvSelectedCard_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
//    {
//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();
//    }
//    protected void dvSelectedCard_ItemCommand(object sender, DetailsViewCommandEventArgs e)
//    {
//        if (e.CommandName == "Cancel")
//        {
//            gvObjects.SelectedIndex = -1;
//            //gvObjects.DataBind();
//        }
//    }
//    protected void dvSelectedCard_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
//    {
//        int CardId = (int)dvSelectedCard.DataKey.Value;

//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();
//    }
//    protected void dvSelectedCard_DataBound(object sender, EventArgs e)
//    {
//        //Card card = dvSelectedCard.DataItem as Card;
//        //if (card == null)
//        //{
//        //    return;
//        //}

//        //DropDownList ddl =
//        //    dvSelectedCard.FindControl("ddlDVOffices") as DropDownList;
//        //if (ddl != null)
//        //{
//        //    ddl.SelectedValue = card.OfficeID.ToString();
//        //}

//        //ddl = dvSelectedCard.FindControl("ddlDVOwners") as DropDownList;
//        //if (ddl != null)
//        //{
//        //    if (card.OwnerID == null)
//        //        ddl.SelectedValue = "0";
//        //    else
//        //        ddl.SelectedValue = card.OwnerID.ToString();
//        //}
//    }
//    protected void dvSelectedCard_ItemInserting(object sender, DetailsViewInsertEventArgs e)
//    {
//        if (!m_CanEdit)
//        {
//            e.Cancel = true;
//            return;
//        }

//        DropDownList ddl = dvSelectedCard.FindControl("ddlDVOffices") as DropDownList;
//        if (ddl != null)
//        {
//            e.Values["OfficeID"] = Convert.ToInt32(ddl.SelectedValue);
//        }

//        ddl = dvSelectedCard.FindControl("ddlDVOwners") as DropDownList;
//        if (ddl != null)
//        {
//            int owner_id = Convert.ToInt32(ddl.SelectedValue);
//            if (owner_id == 0)
//                e.Values["OwnerID"] = null;
//            else
//                e.Values["OwnerID"] = owner_id;
//        }
//    }
//    protected void dvSelectedCard_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
//    {
//        if (!m_CanEdit)
//        {
//            e.Cancel = true;
//            return;
//        }

//        DropDownList ddl = dvSelectedCard.FindControl("ddlDVOffices") as DropDownList;
//        if (ddl != null)
//        {
//            e.NewValues["OfficeID"] = Convert.ToInt32(ddl.SelectedValue);
//        }

//        ddl = dvSelectedCard.FindControl("ddlDVOwners") as DropDownList;
//        if (ddl != null)
//        {
//            int owner_id = Convert.ToInt32(ddl.SelectedValue);
//            if (owner_id == 0)
//                e.NewValues["OwnerID"] = null;
//            else
//                e.NewValues["OwnerID"] = owner_id;
//        }
//    }
//    #endregion


//    #region Events of pager controls
//    protected virtual void OnPageIndexChanged(object sender, EventArgs e)
//    {
//        DropDownList ddl = (DropDownList)sender;

//        gvObjects.PageIndex = Convert.ToInt32(ddl.SelectedValue) - 1;
//    }

//    protected virtual void OnPageSizeChanged(object sender, EventArgs e)
//    {
//        DropDownList ddl = (DropDownList)sender;

//        gvObjects.PageIndex = 0;
//        gvObjects.PageSize = Convert.ToInt32(ddl.SelectedValue);
//    }
//    #endregion

//    #region Methods

//    private void applyCardSettings()
//    {
//        //divAdminOPrerations.Visible = false;
//        lblObjectSearch.Text = (string)GetLocalResourceObject("CardSearch.Text");
//        trAuthors.Visible = false;

//        trManufacturers.Visible = false;

//        trAnnotation.Visible = false;
//        trPublishingYear.Visible = false;

//        trValuePercent.Visible = true;
//        trShopName.Visible = true;
//        trShopSite.Visible = true;

//        trBookThemes.Visible = false;
//        trLanguages.Visible = false;
//        trIsElectronic.Visible = false;
//        trIsPaper.Visible = false;

//        gvObjects.DataSourceID = "dsCards";
//        gvObjects.EmptyDataText = (string)GetLocalResourceObject("NoCards");

//        gvObjects.Columns[0].Visible = false;//Authors
//        gvObjects.Columns[1].Visible = false;//Manufacturers
//        gvObjects.Columns[2].Visible = true;//Title
//        gvObjects.Columns[3].Visible = false;//PublishingYear
//        gvObjects.Columns[4].Visible = false;//Download
//        gvObjects.Columns[5].Visible = true;//ShopName
//        gvObjects.Columns[6].Visible = true;//ShopSite
//        gvObjects.Columns[7].Visible = true;//Edit
//        gvObjects.Columns[8].Visible = true;//Delete

//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();

//        if (m_CanEdit)
//            dvSelectedCard.ChangeMode(DetailsViewMode.Insert);
//        else
//            dvSelectedCard.ChangeMode(DetailsViewMode.ReadOnly);

//        //dvSelectedCard.DataBind();

//        dvSelectedBook.Visible = false;
//        dvSelectedDisk.Visible = false;
//        dvSelectedCard.Visible = true;
//    }

//    private void applyDiskSettings()
//    {
//        //divAdminOPrerations.Visible = false;
//        lblObjectSearch.Text = (string)GetLocalResourceObject("DiskSearch.Text");
//        trAuthors.Visible = false;

//        trManufacturers.Visible = true;

//        trAnnotation.Visible = true;
//        trPublishingYear.Visible = true;

//        trValuePercent.Visible = false;
//        trShopName.Visible = false;
//        trShopSite.Visible = false;

//        trBookThemes.Visible = false;
//        trLanguages.Visible = false;
//        trIsElectronic.Visible = false;
//        trIsPaper.Visible = false;

//        gvObjects.DataSourceID = "dsDisks";
//        gvObjects.EmptyDataText = (string)GetLocalResourceObject("NoDisks");

//        gvObjects.Columns[0].Visible = false;//Authors
//        gvObjects.Columns[1].Visible = true;//Manufacturers
//        gvObjects.Columns[2].Visible = true;//Title
//        gvObjects.Columns[3].Visible = true;//PublishingYear
//        gvObjects.Columns[4].Visible = false;//Download
//        gvObjects.Columns[5].Visible = false;//ShopName
//        gvObjects.Columns[6].Visible = false;//ShopSite
//        gvObjects.Columns[7].Visible = true;//Edit
//        gvObjects.Columns[8].Visible = true;//Delete

//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();

//        if (m_CanEdit)
//            dvSelectedDisk.ChangeMode(DetailsViewMode.Insert);
//        else
//            dvSelectedDisk.ChangeMode(DetailsViewMode.ReadOnly);

//        //dvSelectedDisk.DataBind();

//        dvSelectedBook.Visible = false;
//        dvSelectedDisk.Visible = true;
//        dvSelectedCard.Visible = false;
//    }

//    private void applyBookSettings()
//    {
//        //divAdminOPrerations.Visible = m_CanEdit;
//        lblObjectSearch.Text = (string)GetLocalResourceObject("BookSearch.Text");
//        trAuthors.Visible = true;

//        trManufacturers.Visible = false;

//        trAnnotation.Visible = true;
//        trPublishingYear.Visible = true;

//        trValuePercent.Visible = false;
//        trShopName.Visible = false;
//        trShopSite.Visible = false;

//        trBookThemes.Visible = true;
//        trLanguages.Visible = true;
//        trIsElectronic.Visible = true;
//        trIsPaper.Visible = true;

//        gvObjects.DataSourceID = "dsBooks";
//        gvObjects.EmptyDataText = (string)GetLocalResourceObject("NoBooks");

//        gvObjects.Columns[0].Visible = true;//Authors
//        gvObjects.Columns[1].Visible = false;//Manufacturers
//        gvObjects.Columns[2].Visible = true;//Title
//        gvObjects.Columns[3].Visible = true;//PublishingYear
//        gvObjects.Columns[4].Visible = true;//Download
//        gvObjects.Columns[5].Visible = false;//ShopName
//        gvObjects.Columns[6].Visible = false;//ShopSite
//        gvObjects.Columns[7].Visible = true;//Edit
//        gvObjects.Columns[8].Visible = true;//Delete

//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();

//        if (m_CanEdit)
//            dvSelectedBook.ChangeMode(DetailsViewMode.Insert);
//        else
//            dvSelectedBook.ChangeMode(DetailsViewMode.ReadOnly);

//        //dvSelectedBook.DataBind();

//        dvSelectedBook.Visible = true;
//        dvSelectedDisk.Visible = false;
//        dvSelectedCard.Visible = false;
//    }

//    /// <summary>
//    /// Shows correct pager information.
//    /// </summary>
//    /// <param name="pagerRow">Pager row.</param>
//    private void ShowPagerData(Control pagerRow)
//    {
//        if (pagerRow == null)
//            return;

//        DropDownList ddlPages = pagerRow.FindControl("ddlPage") as DropDownList;
//        Literal lbl = pagerRow.FindControl("lblPageCount") as Literal;
//        DropDownList ddlPSize = pagerRow.FindControl("ddlPageSize") as DropDownList;

//        if ((ddlPages != null) && (lbl != null) && (ddlPSize != null))
//        {
//            ddlPages.Items.Clear();
//            for (int pageIndex = 1; pageIndex <= gvObjects.PageCount; pageIndex++)
//            {
//                ListItem item = new ListItem(pageIndex.ToString());
//                if (pageIndex == gvObjects.PageIndex + 1)
//                    item.Selected = true;
//                ddlPages.Items.Add(item);
//            }

//            lbl.Text = gvObjects.PageCount.ToString();

//            ddlPSize.SelectedValue = gvObjects.PageSize.ToString();
//        }
//    }
//    #endregion

//    protected void btnSearch_Click(object sender, EventArgs e)
//    {
//        gvObjects.SelectedIndex = -1;
//        //gvObjects.DataBind();

//        if (m_CanEdit)
//        {
//            dvSelectedBook.ChangeMode(DetailsViewMode.Insert);
//            dvSelectedDisk.ChangeMode(DetailsViewMode.Insert);
//            dvSelectedCard.ChangeMode(DetailsViewMode.Insert);
//        }
//        else
//        {
//            dvSelectedBook.ChangeMode(DetailsViewMode.ReadOnly);
//            dvSelectedDisk.ChangeMode(DetailsViewMode.ReadOnly);
//            dvSelectedCard.ChangeMode(DetailsViewMode.ReadOnly);
//        }
//    }
//    protected void gvObjects_RowDataBound(object sender, GridViewRowEventArgs e)
//    {
//        //switch (ddlObjectTypes.SelectedIndex)
//        //{
//        //    case 0:
//        //        Book b = e.Row.DataItem as Book;
//        //        if (b == null)
//        //            return;
//        //        ((Literal)e.Row.FindControl("lblGVAuthors")).Text = b.Authors;
//        //        ((Literal)e.Row.FindControl("lblGVTitle")).Text = b.Title;
//        //        ((Literal)e.Row.FindControl("lblGVPublishingYear")).Text = b.PublishingYear.ToString();
//        //        break;
//        //    case 1:
//        //        Disk d = e.Row.DataItem as Disk;
//        //        if (d == null)
//        //            return;
//        //        ((Literal)e.Row.FindControl("lblGVManufacturers")).Text = d.Manufacturers;
//        //        ((Literal)e.Row.FindControl("lblGVTitle")).Text = d.Title;
//        //        ((Literal)e.Row.FindControl("lblGVPublishingYear")).Text = d.PublishingYear.ToString();
//        //        break;
//        //    case 2:
//        //        Card c = e.Row.DataItem as Card;
//        //        if (c == null)
//        //            return;
//        //        ((Literal)e.Row.FindControl("lblGVTitle")).Text = c.Title;
//        //        ((Literal)e.Row.FindControl("lblGVShopName")).Text = c.ShopName;
//        //        ((Literal)e.Row.FindControl("lblGVShopSite")).Text = c.ShopSite;
//        //        break;
//        //}
    //}
}
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using UlterSystems.PortalLib.NewsTape;
using UlterSystems.PortalLib.NewsManager;
using UlterSystems.PortalLib.DB;
using UlterSystems.PortalLib.BusinessObjects;
using Core;
public partial class NewsTape_SearchNews : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        //устанавливаем источник данных для NewsGridView
        gvNews.RequestDatasource += delegate(object gridSender, PagingArgs args)
        {
            PagingResult result = NewsManager.SearchNews(
                args,
                this.tbSearchText.Text,
                Int32.Parse(this.ddlAuthors.SelectedItem.Value),
                (DBManager.NewsStatus)this.ddlNewsStatus.SelectedIndex,
                CurrentUser.ID.Value,
                Int32.Parse(ddlOffices.SelectedItem.Value),
                (DBManager.SearchPeriod)this.ddlPeriod.SelectedIndex
                );
            return result;
        };

        if (!(this.Page.IsPostBack))
        {
            ddlAuthors.Items.Clear();
            Person[] newsEditorsList = UserList.GetGeneralNewsEditorsList();
            ddlAuthors.Items.Add(new ListItem((String)this.GetLocalResourceObject("liAll.Text"),"0"));
            foreach (Person newsEditor in newsEditorsList)
            {
                ddlAuthors.Items.Add(new ListItem(newsEditor.FullName, newsEditor.ID.ToString()));
            }

            ddlOffices.Items.Clear();
            Office[] offices = Office.GetUserOffices(CurrentUser.ID.Value);
            ddlOffices.Items.Add(new ListItem((String)this.GetLocalResourceObject("liAll.Text"), "-1"));
            ddlOffices.Items.Add(new ListItem((String)this.GetLocalResourceObject("liGeneralNews.Text"), "0"));
            foreach (Office office in offices)
            {
                ddlOffices.Items.Add(new ListItem(office.OfficeName, office.ID.ToString()));
            }

        }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        
        gvNews.Visible = true;
        gvNews.RefreshData(true);
        
    }



}
